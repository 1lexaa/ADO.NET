﻿CREATE TABLE Sales (Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, ProductId UNIQUEIDENTIFIER NOT NULL REFERENCES Products(Id), 
					ManagerId UNIQUEIDENTIFIER NOT NULL REFERENCES Managers(Id), Cnt INT NOT NULL DEFAULT 1, SaleDt DATETIME DEFAULT CURRENT_TIMESTAMP, 
					DeleteDt DATETIME NULL)