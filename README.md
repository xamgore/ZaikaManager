Project is a simplified implementation of sales accounting system. It uses PostgreSQL (on MicroLite ORM) to store data, C# WPF with Material design theme to draw user interface. This project is the first one, where I tried to use ORM, WPF, `async/await` and other C# 6.0 features.

<img src="http://i.imgur.com/vSuTdSc.png" width="250">

### How to run

Move to `C:\Program Files\PostgreSQL\9.5\bin` or add this folder to PATH.

```bash
$ createdb  -U postgres     zaika
$ psql      -U postgres  -d zaika
=> \i structure.sql; \i functions.sql; \i data.sql
```
