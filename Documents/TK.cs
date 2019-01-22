	#region Operations
/*  ------------------
	
	Операции сотрудников:
	
		1.	Верт. дефрагментация ( max_duration: )
		2.	Выгрузка машины
		3.	Гор. деврагментация
		4.	Подтоварка
		5.	Инвентаризация
		6.	Комплектация грузовых мест (не используется)
		7.	Консолидация грузовых мест (не используется)
		8.	Обработка грузовых мест (не используется)
		9.	Перемещение товара
		10.	Погрузка машины
		11.	Подбор клиентского товара
		12. Подбор товара
		13.	Подбор товаров покупателей
		14.	Предварительный подбор товара
		15.	Прием груза на терминале (не используется)
		16.	Прочие операции
		17.	Размещение товара
		18.	Размещение товаров покупателей (не используется)
		19.	Расконсолидация грузовых мест (не используется)
		20.	Сканирование товара
		21. Сканирование транзитов
		22.	Упаковка авиагрузов (не используется)
		23.	Упаковка товара в места
		24. Перерыв (не используется)
		
		Итого: 16 из 24-х
		
		Общие характеристики:
		
			-	сотрудник
			-	время начала операции
			-	длительность
			-	документ 1С базы.
*/
	#endregion
	
	
	#region Operation Classifiers
/*	-----------------------------
	
	Классификация операций:
	
		1.	Приём
		
			-	Сканирование товара
			-	Сканирование транзитов
			
		2.	Расстановка и подтоварка
		
			-	Верт. дефрагментация
			-	Гор. деврагментация
			-	Подтоварка
			-	Перемещение товара
			-	Размещение товара
			
		3.	Набор
		
			-	Подбор клиентского товара
			-	Подбор товара
			-	Подбор товаров покупателей
			-	Предварительный подбор товара
			-	Упаковка товара в места
		
		4.	Инвентаризация
		
			-	Инвентаризация
			
		5.	Погрузка/Выгрузка
		
			-	Выгрузка машины
			-	Погрузка машины
			
		6.	Прочие операции
		
			-	Прочие операции
*/
	#endregion
	
		
	#region Actions
/*	------------------------
	
	Actions:

        0.      EmployeeAction:

                    Common characteristics:

                        -   Employee
                        -   StartTime
                        -   Operation
                        -   Duration

	
		I.		Ations with Products and Addresses:
					
					1)  Верт. дефрагментация
					2)  Гор. деврагментация
					3)  Подтоварка
					4)  Инвентаризация
					5)  Перемещение товара
					6)  Подбор клиентского товара
					7)  Подбор товара
					8)  Подбор товаров покупателей
					9)  Предварительный подбор товара
					10) Размещение товара
					11) Сканирование товара
					12) Сканирование транзитов
					13) Упаковка товара в места
					
					Общие характеристики:
					
						-	ActionDetails ( [ Product: Count ] )

					
					Операции с товаром и адресами делятся:
				
						a.	Операции с одним адресом
						
							1) Инвентаризация ( AccountingQuantity )

							2) Сканирование товара ( ScanQuantity )
							3) Сканирование транзитов ( ScanQuantity )
							
							Общие характеристики:
							
								-	Адрес
						
						
						b.	Операции с двумя адресами
						
							1) Верт. дефрагментация
							2) Гор. деврагментация
							3) Подтоварка
							4) Перемещение товара
							5) Подбор клиентского товара (допы)
							6) Подбор товара
							7) Подбор товаров покупателей (К00-00-00-00)
							8) Размещение товара
							9) Упаковка товара в места
							
							Общие характеристики:
							
								-	Адрес-отправитель
								-	Адрес-получатель
					
		II.		Операции ПРР
		
					1) Выгрузка машины
					2) Погрузка машины
					
					Общие характеристики:
					
						-	вес
						-	объём
						-	ГМ с номерами
						-	ГМ без номеров
					
		III.	Остальные операции.
		
					1) Прочие операции
*/				
	#endregion
					
					
	#region Operation Duration Computing
/*	------------------------------------
	
	Operations are ordered by start DateTime
	
	St[n-1]  Operation[n-1]  Et[n-1]         St[n]  Operation[n]  Et[n]        
	....|________________________|..............|____________________|.......
	              d[n-1]                                 d[n]

		1) Operation[n-1] is null:
		
			d[n] = d[n]
	

		2) Operation[n-1] have different classifier:
	
			d[n] = 
	
	
	
	Breaks Check:
	
		Break durations substract from intervals between operations.
*/	
	#endregion