# Cubi12 - User Management Microservice

Este microservicio se encarga de la gestión de usuarios en un entorno de arquitectura distribuida utilizando RabbitMQ y gRPC. Se integra con otros servicios a través de un sistema de mensajería basado en RabbitMQ y permite la consulta y actualización de perfiles de usuario mediante gRPC.

## Requisitos Previos

- **Docker** y **Docker Compose** para ejecutar el servicio en contenedores.
- **RabbitMQ** y **PostgreSQL** configurados para la mensajería y la persistencia de datos.
- **.NET 8 SDK** para desarrollar y ejecutar localmente.

## Configuración y Ejecución

1. **Clonar el repositorio**:

   ```bash
   git clone https://github.com/DavidZeballos/ADSTaller2_GestionUsuarios.git
   cd src
   ```

2. **Configurar el archivo Docker Compose**:

   El archivo `docker-compose.yml` ya contiene las configuraciones para levantar `RabbitMQ`, `PostgreSQL` y el microservicio. Puede personalizar las variables de entorno según sea necesario.

3. **Configurar el archivo de configuración (appsettings.json)**:

   El archivo `appsettings.json` contiene la configuración de conexión a RabbitMQ y a la base de datos PostgreSQL. Es importante que los valores de conexión coincidan con los que están definidos en `docker-compose.yml`.

4. **Ejecutar el servicio**:

   ```bash
   docker-compose up --build
   ```

---

## Descripción del Microservicio

El microservicio expone las siguientes funcionalidades:

### gRPC Endpoints

- **GetUserProfile**: Recupera el perfil de un usuario por su ID.
- **UpdateUserProfile**: Actualiza el perfil de un usuario.

Estos endpoints están implementados en `UserGrpcService` y están accesibles a través de la URL `http://localhost:5000` cuando el servicio está en ejecución.

### Mensajería con RabbitMQ

Cada vez que se registra un usuario en el sistema (desde otro microservicio), se envía un evento `UserCreatedEvent` a través de RabbitMQ. Este evento es consumido por el `RabbitMqConsumerService`, que procesa el mensaje y crea el usuario en la base de datos.

### Ejecución de Pruebas de Rendimiento

Para evaluar el rendimiento del microservicio, se incluye un archivo de **JMeter** (`UserServicePlan.jmx`). Este archivo está preconfigurado con pruebas de carga para los endpoints gRPC y permite simular múltiples solicitudes de usuario para medir la capacidad de respuesta del servicio.

1. **Abrir el archivo en JMeter**:
   - Inicia JMeter y cargue el archivo `UserServicePlan.jmx` ubicado en la carpeta raíz del proyecto.

2. **Ejecutar las pruebas**:
   - Configure los parámetros de la prueba según sus necesidades (por ejemplo, el número de hilos y duración).
   - Ejecute la prueba y observe los resultados en los reportes de JMeter.

#### Simulación de Creación de Usuario

Para pruebas o simulaciones, puede usar el bloque comentado en `Program.cs` para enviar un evento `UserCreatedEvent` a RabbitMQ. Es posible ajustar este bloque para pruebas de desarrollo.

```csharp
// Simulación de creación de usuario en RabbitMQ (comentada para uso futuro)
/*
var rabbitMqService = app.Services.GetService<RabbitMqService>();
if (rabbitMqService != null)
{
    var userId = Guid.NewGuid();
    var userCreatedEvent = new UserCreatedEvent(
        userId,
        "John",              // Nombre
        "Doe",               // Primer apellido
        "Smith",             // Segundo apellido
        "12345678-9",        // RUT
        "john.doe@example.com" // Correo electrónico
    );
    rabbitMqService.Publish(userCreatedEvent);
}
*/
```
