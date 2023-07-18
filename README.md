# FileStorage
# Задание
![image](https://github.com/Lexus39/FileStorage/assets/95744118/cfa473c3-54fb-4aad-b25c-4e5f920252be)
# Реализация
Я решил хранить файлы в файловой системе(папке Development\Files в проекте FileStorage.API). В качестве субд я выбрал PostgreSQL. В бд хранится информация, необходимая для доступа к загруженным файлам. Одноразовые ссылки также хранятся в бд. Потестировать API можно с помощью swagger.
![image](https://github.com/Lexus39/FileStorage/assets/95744118/6848284e-2f9d-4f1e-b637-4d68c52d7b9e)
# Запуск
1) Изменить строку подключения в файле appsetings.json и провайдера для ef(если необходимо);
   ![image](https://github.com/Lexus39/FileStorage/assets/95744118/4986e5ea-91bf-4b27-bfce-a1620ddb1639)
2) Выбрать запускаемый проект FileStorage.API;
3) Применить миграции;
4) Создать папку Development\Files в папке с проектом FileStorage.API.
