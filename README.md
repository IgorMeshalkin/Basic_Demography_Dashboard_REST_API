Что это?
-----------
Это REST API дашборда "Основные демографические показатели Хабаровского края", доступного по адресу:<br>
https://basic-dmg.medkhv.ru/

Описание?
-----------------
API имеет следующие конечные точки: <br><br>
[GET] /districts - получить список районов с данными.<br>
[GET] /districts/without_data - получить "легкий" список районов. Без данных.<br>

[GET] /periods - получить список периодов.<br>
[GET] /periods/united - получить список периодов объединённых вместе если год уже закончился.<br>
[POST] /data - создать период <br>

[GET] /indicators - получить список индикаторов.<br>

[GET] /data?districtID=&periodID= - получить данные по Id района и периода <br>
[POST] /data - изменить данные <br>

[POST] /users - проверить пароль для изменения данных<br>

Технологии
-----------------
API написан чистом C#. Использавана СУБД PostgreSQL.

Контакты
-----------
Меня зовут Игорь Мешалкин,   <br> буду рад замечаниям и предложениям по поводу своей работы.   <br>
Telegram: @IgorMeshalkin   <br>
Email: 770190@bk.ru
