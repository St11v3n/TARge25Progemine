using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TARge25Shop.SeleniumTesting
{
    //järjesti klass, mis omab omadusi mille järgi teste saab järjestada
    public class OrderAttribute : Attribute
    {
        public int Value { get; } //väärtus
        public OrderAttribute(int value) //lihtne omistuskonstruktor
        {
            Value = value;
        }
    }

    //Xunit järjesti mis implementeerib ITestCaseOrdereri liidesest "OrderTestCases" meetodi, mis on see mis testide käivitamisel
    //järjestab nad vastavalt "Order" omadusele
    public class PriorityOrderer : ITestCaseOrderer
    {
        //meetod võtab sisse kõik test cased
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            //meetod tagastab kõik test cased järjestatuna arendaja defineeritud järjesti järgi.
            return testCases.OrderBy(test =>
            {
                //leitakse omadus                                               omaduse tüüp
                var method = test.TestMethod.Method.GetCustomAttributes(typeof(OrderAttribute)).FirstOrDefault();
                //kui ei leita
                if (method == null)
                {
                    //antakse maksimaalne väärtus, ehk test teostatakse kohe, ega ei
                    //panda teiste pontensiaalselt järjekorras olevate testide vahele
                    return int.MaxValue;
                }
                //kui leitakse
                else
                {
                    //siis tagastatakse omaduse "Order" väärtus
                    return method.GetNamedArgument<int>(nameof(OrderAttribute.Value));
                }
            }).ToList(); //pannakse loendisse, ToList on linq meetod.
        }
    }
    internal class KindergartenFrontendTest
    {
    }
}
