# RestroLogic 🍽️

Proyecto base de backend para gestión de restaurantes, construido con **.NET 8**, arquitectura limpia por capas, y buenas prácticas de desarrollo profesional.

## 🚀 Características principales

- **ASP.NET Core Web API** para exponer endpoints RESTful.
- Arquitectura **modular** con capas: Domain, Application, Infrastructure, WebApi.
- **Entity Framework Core** (Code First) para acceso y gestión de base de datos SQL Server.
- Uso de **Value Objects** y patrones DDD.
- Controladores y endpoints documentados con **Swagger/OpenAPI**.
- Proyecto listo para pruebas unitarias, escalabilidad y CI/CD.

## 🏗️ Estructura de carpetas
```plaintext
RestroLogic.Domain           // Entidades, value objects, interfaces, lógica de negocio pura
RestroLogic.Application      // Servicios de aplicación, casos de uso, interfaces de servicios
RestroLogic.Infrastructure   // Repositorios, DbContext, migraciones, acceso a datos
RestroLogic.WebApi           // API REST, controladores, configuración, documentación
```

⚙️ Requisitos
.NET 8 SDK

SQL Server (Express o superior)

Visual Studio 2022 (opcional pero recomendado)

🔧 Instalación y ejecución local
Clona el repositorio

```plaintext
bash
Copiar
Editar
git clone https://github.com/tu-usuario/RestroLogic.git
cd RestroLogic
Configura la cadena de conexión
```

Modifica el archivo RestroLogic.WebApi/appsettings.json con los datos de tu servidor SQL Server:

```plaintext
json
Copiar
Editar
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=RestroLogicDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

Ejecuta las migraciones y crea la base de datos

```plaintext
bash
Copiar
Editar
dotnet ef database update --project RestroLogic.Infrastructure --startup-project RestroLogic.WebApi
```

Levanta la API

```plaintext
bash
Copiar
Editar
cd RestroLogic.WebApi
dotnet run
```

Prueba la API

Accede a Swagger en: https://localhost:7174/swagger

🧑‍💻 Contribuciones
¡Pull requests y sugerencias son bienvenidas!
Por favor abre un issue para discutir mejoras, bugs o nuevas funcionalidades.

📜 Licencia
Este proyecto está licenciado bajo MIT.
Consulta el archivo LICENSE para más detalles.

Desarrollado por Luis Acuña.
Inspirado en buenas prácticas de arquitectura, DDD y clean code.



