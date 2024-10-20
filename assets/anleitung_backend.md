# Anleitung für Datenbank (solange nicht gehostet); von Melanie Dohr; am 20.10.2024
1. Docker Desktop und tadeot container, welcher auf MySQL image mit :latest basiert, erstellen: 
    - Ports: 4100, 41000
    - env Variable: 
        - MYSQL_USER=user
        - MYSQL_PASSWORD=test
        - MYSQL_ROOT_PASSWORD=test 
2. Container run
3. Datagrip öffnen und mit DB verbinden
    - Configure Data source
    - Host: 172.0.0.1
    - User: user, Password: test
4. init_script.sql ausführen
