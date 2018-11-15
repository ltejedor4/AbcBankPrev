# Modelamiento y Validación de Arquitectura

El siguiente proyecto pretende modelar e implementar una solución de Arquitectura utilizando una aproximación Orientada a servicios utilizando los principios de diseño de servicios, diseño de patrones y estrategias para la construcción de arquitectura orientada a microservicios.

# Contexto 

Se desea modelar la arquitectura de un sistema que mejore los metodos de pago de servicios públicos que actualmente realiza el banco ABC, llevándolo a un proceso más ágil para responder a las nuevas necesidades del mercado. Existen convenios con distintos Proveedores de servicio (Agua, Luz, Gas, Telefonía), para que los usuarios realicen los pagos a través de diferentes canales de servicio (Cajeros Automáticos, de Oficina, Telefono, Portales Web, Aplicación Movil).

Se desea que el Banco pueda adicionar nuevos convenios con otros Operadores. Se tendran las siguientes operaciones que podrán soportar cada uno delos convenios:
● Consulta de saldo a Pagar  
● Pago del Servicio  
● Compensación de pago 

Se desea  un conjunto de servicios que representen las necesidades internas del Banco con lo que se permita desacoplar los servicios de los Proveedores y así no depender de sus detalles.  

## Indice de temas a tratar
1. [Arquitectura utilizada](#arquitectura-utilizada)
2. [Patrones de Diseño](#patrones-de-diseño)
3. [Estilo y uso de Servicios](#estilo-y-uso-de-servicios)
4. [Manejo de Contratos, esquemas y políticas](#Manejo-de-Contratos,-esquemas-y-políticas)
5. [Ventajas](#ventajas)
6. [Desventajas](#desventajas)
7. [Implementación](#implementación)
8. [Funcionamiento y virtualización con Docker](#funcionamiento-y-virtualización-con-Docker)
9. [Conclusión](#conclusión)
10. [Presentado](#presentado)

## Estilo de Arquitectura
La Arquitectura de la solución presentada esta basada principalmente en el Patron Nuclear API Gateway con el cual se busca ocultar los microservicios que ofrecen las funcionalidades al cliente dejando un único Endpoint para que ellos se comuniquen. Todas las solicitudes entrantes seran enrutadas hacia los servicios especificos. Ver [aquí](https://github.com/whmarti/MateboMicroservicios/images/Arq_Gateway.JPG) el modelo específico.
![Arqui](https://github.com/whmarti/MateboMicroservicios/images/DiagComponentes_v1.png)
## Patrones de Diseño
Otros patrones tambien manejados: Logic Centralization para evitar redundancia de servicio, Concurrent Contracts en conjunto con Service Facade con la finalidad de soportar nuevos Operadores y apoyar los requerimientos de acoplamiento y abstracción por multiples consumidores.

## Estilo y uso de Servicios
Una vez el usuario realiza alguna de las operaciones a su disposición, a través del Api-Gateway se trasladará la petición dentro del modulo interno de Servicio de Pago, quien hará un envio de mensaje a Kafka para que utilizando la composición de servicios basada en Orquestación, solicite al Dispatcher resuelva a que operador (Convenio) le corresponde atender la operación (utilizando el servicio Routing) y luego invoque (Utilizando el servicio Dispatching) el servicio que corresponda, para lo cual se apoyará en el servicio (Transform) con e fin de trasformar el mensaje en el formato en el que el Proveedor que recibirá la petición.
![Uso](https://github.com/whmarti/MateboMicroservicios/images/Implmenta2.JPG)

## Manejo de Contratos, esquemas y políticas
Se utiliza la herramienta gestora de APIs WSO2 la cual ayuda a la consecución de accesos y a la gobernabilidad y el analisis de todas las APIs allí almacenadas. Aquí se maneja desde su diseño (a nivel de (Contratos) la gestación y creación de cada uno de los contratos, sus esquemas, las politicas de archivado, ciclo de vida, los sitios permitidos desde donde estaran publicados en ambiente de pruebas y de producción, los canales por los cuales se podran consumir, se manejaran los versionamientos de cada uno, los tiempos de timeout, la documentación de cada servicio, usuarios que pueden tener acceso de consumo, etc.
![Inventory Manager](https://github.com/whmarti/MateboMicroservicios/images/Inventary_Mger.JPG)

## Ventajas
El utilizar este tipo de arquitectura API-Gatewaycontribuye a los siguientes beneficios:
-Disminución del intercambio de mensajes entre el cliente y el API del Back-end, que para éste caso son de Proveedores remotos al sistema.

-Interoperabilidad e integración entre sistemas hetereogeneos, facilitando la inclusión de nuevos Proveedores al sistema en tiempo de ejecución, sin requerir especificaciones funcionales complejas.

-Facilidad de mantenimiento de cada operación especifica requerida por un operador o cliente determinado, gracias a su desacoplamiento y especialización funcional.

## Desventajas
Al mismo tiempo al utilizar ésta arquitectura se deben tener en cuenta las siguientes consideraciones:
-Para el consumo de multiples usuarios simultaneos aunque en menor medida que con peticiones a plataformas de clientes convencionales, ya que se posee un cuello de botella a nivel de llamado de microservicios el cual debe ser escalado horizontalmente de forma cuidadosa. Se recomienda tener multiples API gateway.
-Al ser Multi-DataType, se deberá sacrificar un tiempo extra al momento de transformar la información que venga solicitada en un formato y la requiera procesar un Proveedor en otro.

## Implementación
Para la Implementación se trabajó con Ubuntu 18.04 LTE, SqlServer 14.00.30.45, Net Core v 2.1 para el desarrollo de los micro servicios, WSO2 para el manejo de Inventario de Servicios, Portainer I.O para el manejo y monitoreo de Contenedores, instancias y contenedores de Kafka. 
![Portainer.IO](https://github.com/whmarti/MateboMicroservicios/images/Portainer_Kafka.JPG)

## Funcionamiento y virtualización con Docker
### Github
Descargue el proyecto junto con sus sub-proyectos localizados en el repositorio
***** Ejecutar el archivo (.sh) que ponga Luis 
***** Levantar la imagen del WSO2 que envio William
***** Levantar la BD con el Kafka que Pnga Mario

## Conclusión
Podemos observar que el aprovechamiento de metodologias ya analizadas por varios años, nos permite enfrentar inconvenientes que se presentan en los procesos del dia a dia que deben afrontar las organizaciones en la búsqueda de presentar un espectro de servicios mucho mas amplio a los clientes finales, para lograr obtener un mayor beneficio en sus propuestas de valor y una mayor presencia competitiva en el mercado, produciendo al mismo tiempo una mayor oferta de servicios de consumo en la sociedad para lograr evolucionar al final, en los aspectos de la tecnología que nos mantienen a todos nosotros aqui reunidos en formación y aplicación constante mediante propuestas cada vez más livianas e innovadoras.

## Presentado
Esta solución de Arquitectura fue elaborada por los estudiantes de la Especialización de Arquitectura:
-Mario Bonilla
-Luis Tejedor
-William MArtin

 Noviembre 2018 ©