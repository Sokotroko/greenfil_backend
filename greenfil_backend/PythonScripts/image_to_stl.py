import requests
import base64
import time
import pymeshlab
import os
import sys
from stl import mesh
import numpy as np

def main(image_path, output_dir, output_name):
    # 1. Configura tu API Key
    API_KEY = "msy_yLpQ2Qy3nNBt7kFfgSVPU34qOvrHVdWPAycQ"

    # 2. Detectar tipo MIME
    ext = os.path.splitext(image_path)[1].lower()
    if ext == ".png":
        mime_type = "image/png"
    elif ext in [".jpg", ".jpeg"]:
        mime_type = "image/jpeg"
    else:
        raise ValueError("Formato no soportado. Usa PNG, JPG o JPEG.")

    # 3. Codificar imagen a base64
    with open(image_path, "rb") as image_file:
        encoded_image = base64.b64encode(image_file.read()).decode("utf-8")
    data_uri = f"data:{mime_type};base64,{encoded_image}"

    # 4. Configurar solicitud a la API
    headers = {"Authorization": f"Bearer {API_KEY}", "Content-Type": "application/json"}
    payload = {
        "image_url": data_uri,
        "ai_model": "meshy-4",
        "topology": "triangle",
        "target_polycount": 30000,
        "symmetry_mode": "auto",
        "should_remesh": True,
        "should_texture": False,
        "enable_pbr": False,
        "texture_prompt": "realistic metallic look"
    }

    # 5. Enviar solicitud
    response = requests.post(
        "https://api.meshy.ai/openapi/v1/image-to-3d",
        headers=headers,
        json=payload
    )

    if response.status_code not in [200, 202]:
        raise Exception(f"Error en la API: {response.status_code} - {response.text}")

    task_id = response.json()["result"]
    print(f" Tarea creada: {task_id}")

    # 6. Esperar resultado
    while True:
        status_response = requests.get(
            f"https://api.meshy.ai/openapi/v1/image-to-3d/{task_id}",
            headers=headers
        )
        task = status_response.json()
        estado = task.get("status")

        if estado == "SUCCEEDED":
            obj_url = task["model_urls"].get("obj")
            if not obj_url:
                raise Exception("No se encontró el archivo OBJ.")

            # Descargar OBJ
            obj_filename = os.path.join(os.getcwd(), "temp_model.obj")
            with requests.get(obj_url, stream=True) as r:
                r.raise_for_status()
                with open(obj_filename, "wb") as f:
                    for chunk in r.iter_content(chunk_size=8192):
                        f.write(chunk)

            # Convertir a STL
            ms = pymeshlab.MeshSet()
            ms.load_new_mesh(obj_filename)
            stl_filename = os.path.join(os.getcwd(), "temp_model.stl")
            ms.save_current_mesh(stl_filename)

            # Rotar STL
            your_mesh = mesh.Mesh.from_file(stl_filename)
            angle = np.radians(-90)
            rotation_matrix = np.array([
                [1, 0, 0],
                [0, np.cos(angle), -np.sin(angle)],
                [0, np.sin(angle), np.cos(angle)]
            ])
            your_mesh.vectors = np.dot(your_mesh.vectors, rotation_matrix)
            
            rotated_filename = os.path.join(output_dir, output_name)
            your_mesh.save(rotated_filename)

            # Limpiar archivos temporales
            os.remove(obj_filename)
            os.remove(stl_filename)

            return rotated_filename  # Ruta del STL final

        elif estado == "FAILED":
            raise Exception(f"Fallo en la generación: {task.get('task_error', {}).get('message', 'Error desconocido')}")
        
        time.sleep(5)

if __name__ == "__main__":
    if len(sys.argv) < 4:
        print(" Uso: python image_to_stl.py <ruta_imagen> <carpeta_salida> <nombre_archivo>", file=sys.stderr)
        sys.exit(1)

    try:
        image_path = sys.argv[1]
        output_dir = sys.argv[2]
        output_name = sys.argv[3]
        stl_path = main(image_path, output_dir, output_name)
        print(stl_path)
    except Exception as e:
        print(f" Error: {str(e)}", file=sys.stderr)
        sys.exit(1)