Первую часть задания - добиться бана от сайта, с которого собираются данные - выполнить получилось успешно.
При превышении определенного числа запросов сервер перенаправлял мое приложение на страницу с вводом капчи.

Вторая часть задания - разработать решение, которое позволяет собирать данные без получения бана - вызвала затруднение.

Я видел следующие варианты:
1. Использование прокси-сервера - поскольку сайт использует SSL шифрование, то искал прокси-сервер, который работает с протоколом https. В сети большое разнообразие прокси-серверов, но лишь с помощью небольшого числа получилось собирать данные с сайта (ip и порт прокси-сервера захардкожен в Program.cs)
2. Использование API сайта. У cian.ru отсутствует доступное API, этот вариант отпал.
3. Установить таймаут для каждого запроса на сервер.

Остальные варианты обхода защиты, насколько знаю я, не совсем законны (возможно, я ошибаюсь).