# Web-сервис для обмена сообщениями
## Особенности
Сервис работает по протоколу `http`.
Внешние **порты** HTTP по умолчанию:
- клиент: `7103`;
- сервер: `7063`;
- база данных PostgreSQL: `5432`.
## Структура проекта
Состоит из:
- проекта **TestTask.Server** (_ASP.NET Core_): сервер для обработки сообщений с использованием библиотеки SignalR. Получает сообщения через запрос от клиента №1, записывает его в базу данных с помощью методов проекта KoshelekRu_TestTask.Repository и пересылает подключённому по websocket клиенту №2;
- проекта **TestTask.Clients** (_ASP.NET Core MVC+WebAPI_): WebAPI с 2-мя контроллерами (для отправки сообщения и для получения 10 последних сообщений из базы данных) и MVC с HTML-клиентом, устанавливающим websocket-соединение с сервером и получающим от него сообщения;
- проекта **KoshelekRu_TestTask.Repository** (_библиотека классов_): предоставляет классы для работы с базой данных (PostgreSQL);
- проекта **KoshelekRu_TestTask.Domain** (_библиотека классов_): предоставляет классы и интерфейсы уровня домена;
- проекта **TestTask.Configuration** (_библиотека классов_): предоставляет классы для получения общих настроек приложения, хранящихся в конфигурационном файле JSON.

## Развёртывание проекта с помощью docker-compose
Использовать следующую конфигурацию файла _docker-compose.yml_:
```
services:
  postgresql:
    image: postgres
    container_name: postgres
    ports:
      - "5432:5432"
    environment:
      - POSTGRES_PASSWORD=koshelek.Ru@2025
      - POSTGRES_USER=koshelek
    networks:
      - koshelek-network
  testtask.server:
    image: creepmah/testtaskserver
    container_name: testtask-server
    depends_on:
      - postgresql
    ports:
      - "7063:8080"
    networks:
      - koshelek-network
  testtask.clients:
    image: creepmah/testtask-clients
    container_name: testtask-clients
    depends_on:
      - postgresql
      - testtask.server
    ports:
      - "7103:8080"
    networks:
      - koshelek-network
networks:
  koshelek-network:
```
## Запуск проекта
Открыть домашнюю страницу проекта по адресу [http://localhost:7103](http://localhost:7103). Щёлкнуть по ссылкам сервиса описания WebAPI **Scalar** и клиента для получения сообщений.
Отправка сообщений осуществляется с помощью метода WebAPI `SendMessage`.
Получение 10 последних сообщений осуществляется с помощью метода WebAPI `GetLast10Messages`.
