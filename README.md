# SapApiDotNet9

API REST en C# (.NET 9, ASP.NET Core) para conexión a SAP Business One via DI API.

## Endpoints

- `GET /hello`: Devuelve "Hello World from SAP API".
- `POST /auth`: Recibe JSON `{ "id": "usuario", "password": "clave" }` y conecta a SAP Business One usando la DI API (SAPbobsCOM).

## Estructura de archivos

- `Program.cs`: Configuración principal y DI.
- `Controllers/HelloController.cs`: Endpoint /hello.
- `Controllers/AuthController.cs`: Endpoint /auth.
- `Services/ISapConnector.cs`: Interfaz para conexión SAP.
- `Services/SapConnectorReal.cs`: Implementación real usando SAPbobsCOM.

## Publicación para IIS

1. Asegúrate de tener la referencia COM a SAPbobsCOM agregada al proyecto.
2. Publica con:

```bash
dotnet publish -c Release -o ./publish
```

El contenido de `./publish` puede desplegarse en IIS.

### Notas
- Configura correctamente los parámetros de conexión a SAP en `SapConnectorReal.cs` (servidor, tipo de base de datos, etc).
- El proyecto es compatible con Windows Server 2012 y 2025.
- Requiere .NET 9 SDK y la DI API instalada en el servidor.
