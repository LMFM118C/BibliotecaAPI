INSERT INTO Autores (Nombre, Nacionalidad, FechaNacimiento)
VALUES
('J. K. Rowling','Reino Unido','1965-07-31'),
('George R. R. Martin','Estados Unidos','1948-09-20'),
('Brandon Sanderson','Estados Unidos','1975-12-19'),
('Stephen King','Estados Unidos','1947-09-21'),
('J. R. R. Tolkien','Reino Unido','1892-01-03');

INSERT INTO Libros (Titulo,Genero,NumeroPaginas,Precio,Disponible,FechaPublicacion,AutorId)
VALUES
('Harry Potter y la piedra filosofal','Fantasía',320,22.50,1,'1997-06-26',1),
('Harry Potter y la cámara secreta','Fantasía',341,24.95,1,'1998-07-02',1),
('Harry Potter y el prisionero de Azkaban','Fantasía',435,27.50,0,'1999-07-08',1),
('Juego de Tronos','Fantasía',870,39.95,1,'1996-08-06',2),
('Choque de Reyes','Fantasía',960,42.00,0,'1998-11-16',2),
('Tormenta de Espadas','Fantasía',1210,45.90,1,'2000-08-08',2),
('Elantris','Fantasía',650,18.95,1,'2005-04-21',3),
('Nacidos de la Bruma','Fantasía',720,29.90,1,'2006-07-17',3),
('El Camino de los Reyes','Fantasía',1250,44.95,0,'2010-08-31',3),
('It','Terror',1138,34.90,1,'1986-09-15',4),
('El Resplandor','Terror',600,26.95,1,'1977-01-28',4),
('Misery','Suspense',480,21.90,0,'1987-06-08',4),
('El Señor de los Anillos','Fantasía',1390,54.95,1,'1954-07-29',5),
('El Hobbit','Fantasía',310,18.50,1,'1937-09-21',5),
('El Silmarillion','Fantasía',450,25.50,0,'1977-09-15',5)