### How to build

Move to `C:\Program Files\PostgreSQL\9.5\bin` or add this folder to PATH.

```bash
$ createdb  -U postgres     zaika
$ psql      -U postgres  -d zaika
=> \i structure.sql; \i functions.sql; \i data.sql
```
