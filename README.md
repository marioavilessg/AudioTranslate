# Maui1 - AudioTranslate

Una aplicación multiplataforma desarrollada con **.NET MAUI** que permite comunicarse mediante audio bidireccional traducido automáticamente entre diferentes idiomas a través de **RabbitMQ**.

## 📋 Descripción del Proyecto

**AudioTranslate** es una solución empresarial de comunicación que integra:

- 🎙️ **Reconocimiento de voz** - Conversión de audio a texto usando Azure Cognitive Services
- 🌐 **Traducción automática** - Traducción de texto entre múltiples idiomas
- 📞 **Mensajería por colas** - Comunicación asincrónica a través de RabbitMQ

## 🛠️ Stack Tecnológico

### Framework y Lenguaje
- **.NET 9.0** - Framework de aplicación
- **C#** - Lenguaje de programación
- **.NET MAUI** - Framework multiplataforma para UI nativa
- **Blazor Web View** - Componentes web en aplicaciones nativas

### Servicios Externos
- **Azure Cognitive Services**
  - Speech Recognition API (Reconocimiento de voz)
  - Translator API (Traducción de texto)
- **RabbitMQ** - Mensaje broker para comunicación asincrónica

### Frontend
- **Razor Components** (.razor)
- **Tailwind CSS** - Framework de estilos utilitarios
- **HTML/CSS** - Markup y estilos web

### Dependencias Principales
```xml
<PackageReference Include="Microsoft.Maui.Controls" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebView.Maui" />
<PackageReference Include="Microsoft.CognitiveServices.Speech" Version="1.48.2" />
<PackageReference Include="RabbitMQ.Client" />
<PackageReference Include="Plugin.Maui.Audio" />
```

## 📁 Estructura del Proyecto

```
Maui1/
├── Components/                    # Componentes Blazor
│   ├── Pages/
│   │   ├── Chat.razor            # Página principal de chat
│   │   ├── Chat.razor.css        # Estilos del chat
│   │   ├── Settings.razor        # Configuración de RabbitMQ
│   │   └── Settings.razor.css    # Estilos de configuración
│   ├── Layout/
│   │   ├── MainLayout.razor      # Layout principal
│   │   └── NavMenu.razor         # Menú de navegación
│   ├── Routes.razor              # Definición de rutas
│   └── _Imports.razor            # Imports globales
├── Models/                        # Modelos de datos
│   ├── ChatMessage.cs            # Estructura de mensajes
│   ├── LanguageOption.cs         # Opciones de idioma
│   └── RabbitSettings.cs         # Configuración de RabbitMQ
├── Services/                      # Servicios de negocio
│   ├── ConfigurationService.cs   # Gestión de configuración
│   ├── RabbitMqService.cs        # Integración con RabbitMQ
│   ├── SpeechService.cs          # Reconocimiento de voz
│   └── TranslatorService.cs      # Servicio de traducción
├── Styles/                        # Estilos globales
│   └── tailwind.css              # Configuración Tailwind
├── wwwroot/                       # Assets estáticos
│   ├── css/                       # Estilos compilados
│   └── index.html                # Página HTML base
├── Resources/                     # Recursos de la aplicación
│   ├── AppIcon/                  # Icono de aplicación
│   ├── Fonts/                    # Tipografías
│   ├── Images/                   # Imágenes
│   ├── Splash/                   # Pantalla de inicio
│   └── Raw/                      # Assets sin procesar
├── Platforms/                     # Código específico de plataforma
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   ├── Tizen/
│   └── Windows/
├── Properties/
│   └── launchSettings.json       # Configuración de ejecución
├── App.xaml                       # Definición de aplicación
├── App.xaml.cs                    # Lógica de aplicación
├── MainPage.xaml                  # Página principal
├── MainPage.xaml.cs              # Lógica de página principal
├── MauiProgram.cs                # Configuración de inicio
├── appsettings.json              # Configuración de Azure
├── package.json                   # Dependencias npm
├── tailwind.config.js            # Configuración de Tailwind
└── Maui1.csproj                  # Definición del proyecto
```

## 🔧 Arquitectura

### Patrón de Servicios
La aplicación utiliza una arquitectura basada en servicios inyectados mediante **Dependency Injection**:

```csharp
// Registro de servicios en MauiProgram.cs
builder.Services.AddSingleton<ConfigurationService>();
builder.Services.AddSingleton<RabbitMqService>();
builder.Services.AddSingleton<TranslatorService>();
builder.Services.AddSingleton<SpeechService>();
```

### Modelos de Datos

#### ChatMessage
```csharp
public class ChatMessage
{
    public string SenderId { get; set; }
    public string OriginalText { get; set; }
    public string SourceLanguage { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsAudio { get; set; }
    public byte[]? AudioData { get; set; }
    public int AudioDuration { get; set; }
}
```

#### RabbitSettings
```csharp
public class RabbitSettings
{
    public string Host { get; set; }
    public int Port { get; set; } = 5672;
    public string Username { get; set; }
    public string Password { get; set; }
    public string MyQueue { get; set; }
    public string TargetQueue { get; set; }
}
```

## 🔌 Servicios Principales

### 1. ConfigurationService
Gestiona la configuración de RabbitMQ y persiste los datos en las preferencias de la aplicación.

**Funcionalidades:**
- Cargar configuración predeterminada
- Guardar configuración en almacenamiento local
- Notificar cambios de configuración a otros servicios

### 2. RabbitMqService
Maneja la conexión y comunicación con RabbitMQ.

**Funcionalidades:**
- Conexión a servidor RabbitMQ
- Escucha de mensajes en colas
- Envío de mensajes a otras colas
- Gestión de estados de conexión
- Eventos de desconexión y reconexión automática

### 3. SpeechService
Integra Azure Cognitive Services para reconocimiento de voz.

**Funcionalidades:**
- Conversión de audio a texto (Speech-to-Text)
- Síntesis de voz (Text-to-Speech)
- Soporte para múltiples idiomas

**API Keys requeridas:**
- `AzureSpeech:Key` - Clave de suscripción
- `AzureSpeech:Region` - Región de Azure (ej: francecentral)

### 4. TranslatorService
Utiliza Azure Translator API para traducción automática.

**Funcionalidades:**
- Traducción de texto entre idiomas
- Detección automática de idioma

**API Keys requeridas:**
- `AzureTranslator:Key` - Clave de suscripción
- `AzureTranslator:Region` - Región de Azure
- `AzureTranslator:Endpoint` - URL del endpoint API

## 📱 Páginas Principales

### 1. Chat (/)
Interfaz principal para la comunicación bidireccional.

**Características:**
- Visualización de historial de mensajes
- Grabación y envío de audio
- Selector de idioma para traducción
- Indicador de conexión a RabbitMQ
- Reproducción de mensajes recibidos

### 2. Settings (/settings)
Panel de configuración de la conexión RabbitMQ.

**Características:**
- Configuración de host y puerto
- Credenciales (usuario y contraseña)
- Nombre de colas (entrada y salida)
- Indicador de estado de conexión
- Botón para guardar y reconectar

## ⚙️ Configuración

### appsettings.json
```json
{
  "AzureSpeech": {
    "Key": "YOUR_AZURE_SPEECH_KEY",
    "Region": "francecentral"
  },
  "AzureTranslator": {
    "Key": "YOUR_AZURE_TRANSLATOR_KEY",
    "Region": "francecentral",
    "Endpoint": "https://api.cognitive.microsofttranslator.com"
  }
}
```

### Configuración Predeterminada de RabbitMQ
```csharp
Host = "localhost"
Port = 5672
Username = "admin"
Password = "admin"
MyQueue = "cola_A"
TargetQueue = "cola_B"
```

## 🚀 Instalación y Requisitos

### Requisitos Previos
- **.NET 9.0 SDK** o superior
- **Visual Studio 2022** o **Rider** con extensión MAUI
- **RabbitMQ Server** en ejecución
- **Credenciales de Azure** (Cognitive Services)
- **Node.js** (para compilar Tailwind CSS)

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone <repository-url>
cd Maui1
```

2. **Restaurar dependencias .NET**
```bash
dotnet restore
```

3. **Instalar dependencias npm**
```bash
npm install
```

4. **Compilar Tailwind CSS**
```bash
npm run build:css
```

5. **Configurar variables de entorno**
   - Actualizar `appsettings.json` con las claves de Azure
   - Configurar RabbitMQ en la aplicación a través de Settings

6. **Ejecutar la aplicación**
```bash
dotnet build
dotnet run
```

## 📦 Plataformas Soportadas

La aplicación está configurada para ejecutarse en múltiples plataformas:

- **Android** - Versión mínima: 24.0
- **iOS** - Versión mínima: 15.0
- **macOS Catalyst** - Versión mínima: 15.0
- **Windows** - Versión mínima: 10.0.17763.0

## 🎨 Estilos y Diseño

### Tailwind CSS
El proyecto utiliza **Tailwind CSS** para la generación de estilos.

**Scripts disponibles:**
```bash
npm run build:css    # Compilar CSS (producción)
npm run watch:css    # Vigilar cambios en CSS (desarrollo)
```

### Colores Primarios
- Primario: `primary-500`, `primary-600`, `primary-700`
- Escala de grises: `slate-100` a `slate-800`
- Estados: `emerald` (éxito), `red` (error)

## 🔐 Seguridad

### Consideraciones Importantes
1. **Las claves de Azure están expuestas en appsettings.json** - Se recomienda usar Azure Key Vault en producción
2. **Las credenciales de RabbitMQ se almacenan localmente** - Proteger con encriptación adicional
3. **El almacenamiento local de preferencias debe estar cifrado** en aplicaciones con datos sensibles

## 🐛 Solución de Problemas

### Problemas Comunes

**Error de conexión a RabbitMQ**
- Verificar que RabbitMQ está ejecutándose
- Confirmar host, puerto, usuario y contraseña
- Revisar firewall y reglas de red

**Problemas de reconocimiento de voz**
- Verificar permisos de micrófono en el dispositivo
- Comprobar validez de las claves de Azure
- Confirmar la región de Azure configurada

**Estilos Tailwind no aplicados**
- Ejecutar `npm run build:css`
- Limpiar y reconstruir el proyecto
- Verificar que `tailwind.css` se está compilando correctamente

## 📊 Flujo de Comunicación

```
┌─────────────────┐
│   Usuario A     │
│   (Maui1)       │
└────────┬────────┘
         │ Graba audio
         │ (Idioma A)
         │
         ▼
    ┌─────────────────────┐
    │  SpeechService      │ ◄─────────────► Azure Speech API
    │  (Speech-to-Text)   │
    └─────────┬───────────┘
              │ Texto en idioma A
              │
              ▼
         ┌──────────────────────┐
         │ TranslatorService    │ ◄─────────────► Azure Translator API
         │ (A → Idioma B)       │
         └─────────┬────────────┘
                   │ Texto en idioma B
                   │
                   ▼
              ┌──────────────────┐
              │ RabbitMqService  │
              │ (Envía mensaje)  │
              └────────┬─────────┘
                       │ PublishAsync()
                       │
                       ▼
                  ┌──────────────┐
                  │  RabbitMQ    │
                  │  (cola_B)    │
                  └──────────────┘
                       │
                       ▼ ConsumeAsync()
                  ┌──────────────────┐
                  │   Usuario B      │
                  │   (Maui1/Maui2)  │
                  └──────────────────┘
```

## 📝 Notas de Desarrollo

### Debugging
- Usar `#if DEBUG` para código específico de debug
- Las herramientas de desarrollo de Blazor están habilitadas en DEBUG
- Logs disponibles a través de `ILogger<T>`

### Extensiones Futuras
1. Cifrado de mensajes en tránsito
2. Almacenamiento de historial de chat
3. Soporte para más idiomas
4. Grabación de llamadas
5. Notificaciones push
6. Autenticación de usuarios

## 📄 Licencia

[Especificar licencia del proyecto]

## 👥 Contribuciones

[Especificar proceso de contribuciones]

## 📞 Soporte

Para reportar errores o solicitar funcionalidades, por favor crear un issue en el repositorio.

---

**Última actualización:** Marzo 2026
**Versión:** 1.0
**Estado:** En desarrollo

