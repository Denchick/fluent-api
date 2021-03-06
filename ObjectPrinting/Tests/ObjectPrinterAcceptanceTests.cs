﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
	[TestFixture]
	public class ObjectPrinterAcceptanceTests
	{
		[Test]
		public void Demo()
		{
			var person = new Person { Name = "Alex", Age = 19 };

			var printer = ObjectPrinter.For<Person>()
				//1. Исключить из сериализации свойства определенного типа
				.Excluding<Guid>()
				//2. Указать альтернативный способ сериализации для определенного типа
				.Printing<int>().Using(i => i.ToString())
				//3. Для числовых типов указать культуру
				.Printing<double>().Using(CultureInfo.CurrentCulture)
				//4. Настроить сериализацию конкретного свойства
				.Printing(p => p.Age).Using(age => age.ToString())
				//5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
				.Printing(p => p.Name).CutToLenght(3)
				//6. Исключить из сериализации конкретного свойства
				.Excluding(p => p.Name);
            
            string s1 = printer.PrintToString(person);

			//7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию	
			string s2 = person.PrintToString();
			//8. ...с конфигурированием
			string s3 = person.PrintToString(s => s.Excluding(p => p.Age));
		}
	}
}