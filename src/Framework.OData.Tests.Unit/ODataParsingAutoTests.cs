using System;
using System.Diagnostics;
using Framework.Core;
using Framework.QueryLanguage;
using NUnit.Framework;
using FluentAssertions;

namespace Framework.OData.Tests.Unit
{
    //TODO: Move this class to autotests
    [TestFixture]
    public class ODataParsingAutoTests
    {
        [Test]
        public void Test001()
        {
            var skipCount = 123;
            var takeCount = 999;
            var dateStr = "2014-05-01T00:00:00";
            var datePropertyName = "Date";

            var date = DateTime.Parse(dateStr);

            var testStr = $"$skip={skipCount}&$top={takeCount}&$filter={datePropertyName} ge datetime'{dateStr}'";

            var parameter = ParameterExpression.Default;

            var filter = new LambdaExpression(

                            new BinaryExpression(
                                new PropertyExpression(parameter, datePropertyName),
                                BinaryOperation.GreaterThanOrEqual,
                                new DateTimeConstantExpression(date)),

                            parameter);


            var expectedOperation = new SelectOperation(
                filter,
                SelectOperation.Default.Orders,
                SelectOperation.Default.Expands,
                SelectOperation.Default.Selects,
                skipCount,
                takeCount);

            this.Test(testStr, expectedOperation);
        }


        [Test]
        public void Test002()
        {
            var skipCount = 10;
            var takeCount = 20;

            var path00 = "Location";
            var path01 = Tuple.Create("Name", "LN");

            var path10 = "NameNative";
            var path11 = Tuple.Create("FirstName", "FN");

            var testStr = string.Format("$skip={0}&$top={1}&$select={2}/[{3} {4}],{5}/[{6} {7}]&$expand={2},{5}",
                skipCount, takeCount, path00, path01.Item1, path01.Item2, path10, path11.Item1, path11.Item2);

            var parameter = ParameterExpression.Default;


            var select0 = new LambdaExpression(
                            new SelectExpression(
                                new PropertyExpression(parameter, path00), path01.Item1, path01.Item2), parameter);

            var select1 = new LambdaExpression(
                            new SelectExpression(
                                new PropertyExpression(parameter, path10), path11.Item1, path11.Item2), parameter);


            var expand0 = new LambdaExpression(
                            new PropertyExpression(parameter, path00), parameter);

            var expand1 = new LambdaExpression(
                            new PropertyExpression(parameter, path10), parameter);


            var expectedOperation = new SelectOperation(
                SelectOperation.Default.Filter,
                SelectOperation.Default.Orders,
                new[] { expand0, expand1 },
                new[] { select0, select1 },
                skipCount,
                takeCount);

            this.Test(testStr, expectedOperation);
        }

        [Test]
        public void Test003()
        {
            var takeCount = 70;

            var dateStr1 = "2014-05-01T00:00:00";
            var dateStr2 = "2014-05-31T00:00:00";

            var date1 = DateTime.Parse(dateStr1);
            var date2 = DateTime.Parse(dateStr2);

            var datePropertyName = "Date";
            var collectionPropertyName = "ProjectItems";
            var subAlias = "s";
            var projectPropertyName = "Project";
            var codePropertyName = "Code";

            var codeConstValue = "ACERM_1";


            var testStr = string.Format("$top={0}&$filter=((({1} ge datetime'{2}' and {1} le datetime'{3}')) and (any({4} {5}, {5}/{6}/{7} eq '{8}')))",
                takeCount, datePropertyName, dateStr1, dateStr2, collectionPropertyName, subAlias, projectPropertyName, codePropertyName, codeConstValue);

            var parameter = ParameterExpression.Default;

            var subParameter = new ParameterExpression(subAlias);


            var subFilter = new LambdaExpression(

                new BinaryExpression(
                    new PropertyExpression(new PropertyExpression(subParameter, projectPropertyName), codePropertyName),
                    BinaryOperation.Equal,
                    new StringConstantExpression(codeConstValue)),

                subParameter);


            var filter = new LambdaExpression(

                            new BinaryExpression(

                                new BinaryExpression(

                                    new BinaryExpression(
                                        new PropertyExpression(parameter, datePropertyName),
                                        BinaryOperation.GreaterThanOrEqual,
                                        new DateTimeConstantExpression(date1)),

                                    BinaryOperation.AndAlso,

                                    new BinaryExpression(
                                        new PropertyExpression(parameter, datePropertyName),
                                        BinaryOperation.LessThanOrEqual,
                                        new DateTimeConstantExpression(date2))),

                                BinaryOperation.AndAlso,

                                new MethodExpression(
                                    new PropertyExpression(parameter, collectionPropertyName),
                                    MethodExpressionType.CollectionAny,

                                    subFilter)),

                            parameter);


            var expectedOperation = new SelectOperation(
                filter,
                SelectOperation.Default.Orders,
                SelectOperation.Default.Expands,
                SelectOperation.Default.Selects,
                SelectOperation.Default.SkipCount,
                takeCount);

            this.Test(testStr, expectedOperation);
        }

        [Test]
        public void Test004()
        {
            var const1Str = "true";
            var const2Str = "40a7662f-698d-42d9-a9a7-9f0300adc398";

            var const1 = bool.Parse(const1Str);
            var const2 = Guid.Parse(const2Str);


            var prop00 = "IsDefault";

            var prop10 = "Employee";
            var prop11 = "Id";


            var testStr = $"$filter=({prop00} eq {const1Str} and {prop10}/{prop11} eq guid'{const2Str}')";

            var parameter = ParameterExpression.Default;


            var filter = new LambdaExpression(

                            new BinaryExpression(

                                new BinaryExpression(
                                    new PropertyExpression(parameter, prop00),
                                    BinaryOperation.Equal,
                                    new BooleanConstantExpression(const1)),

                                BinaryOperation.AndAlso,

                                    new BinaryExpression(
                                        new PropertyExpression(new PropertyExpression(parameter, prop10), prop11),
                                        BinaryOperation.Equal,
                                        new GuidConstantExpression(const2))),


                            parameter);


            var expectedOperation = new SelectOperation(
                filter,
                SelectOperation.Default.Orders,
                SelectOperation.Default.Expands,
                SelectOperation.Default.Selects,
                SelectOperation.Default.SkipCount,
                SelectOperation.Default.TakeCount);

            this.Test(testStr, expectedOperation);
        }


        [Test]
        public void Test005()
        {
            var constDateStr1 = "2014-05-01T00:00:00";
            var constDateStr2 = "2014-06-01T00:00:00";
            var constDateStr3 = "2014-07-01T00:00:00";

            var constDate1 = DateTime.Parse(constDateStr1);
            var constDate2 = DateTime.Parse(constDateStr2);
            var constDate3 = DateTime.Parse(constDateStr3);

            var constPeriod1 = new Period(constDate1);
            var constPeriod2 = new Period(constDate2, constDate3);

            var prop0 = "Period";


            var testStr = string.Format("$filter=isIntersectedP(period(datetime'{0}'), {1}) and containsP({1}, period(datetime'{2}', datetime'{3}'))",
                constDate1, prop0, constDate2, constDateStr3);


            var parameter = ParameterExpression.Default;

            var filter = new LambdaExpression(

                            new BinaryExpression(

                                new MethodExpression(
                                    new PeriodConstantExpression(constPeriod1),
                                    MethodExpressionType.PeriodIsIntersected,
                                    new PropertyExpression(parameter, prop0)),

                                BinaryOperation.AndAlso,

                                new MethodExpression(
                                    new PropertyExpression(parameter, prop0),
                                    MethodExpressionType.PeriodContains,
                                    new PeriodConstantExpression(constPeriod2))),

                            parameter);


            var expectedOperation = new SelectOperation(
                filter,
                SelectOperation.Default.Orders,
                SelectOperation.Default.Expands,
                SelectOperation.Default.Selects,
                SelectOperation.Default.SkipCount,
                SelectOperation.Default.TakeCount);

            this.Test(testStr, expectedOperation);
        }

        [Test]
        [TestCase("m")]
        [TestCase("M")]
        public void DecimalParse_DifferentMLetterCase_ParseSuccess(string m)
        {
            // Arrange
            var skipCount = 123;
            var takeCount = 999;
            var decimalStr = "10.2" + m;
            var deciamlName = "Param";

            var decimalValue = 10.2m;

            var testStr = $"$skip={skipCount}&$top={takeCount}&$filter={deciamlName} eq {decimalStr}";

            var parameter = ParameterExpression.Default;

            var filter = new LambdaExpression(

                            new BinaryExpression(
                                new PropertyExpression(parameter, deciamlName),
                                BinaryOperation.Equal,
                                new DecimalConstantExpression(decimalValue)),

                            parameter);


            var expectedOperation = new SelectOperation(
                filter,
                SelectOperation.Default.Orders,
                SelectOperation.Default.Expands,
                SelectOperation.Default.Selects,
                skipCount,
                takeCount);

            // Act and Assert
            this.Test(testStr, expectedOperation);
        }

        [Test]
        public void SelectOperation_ParseNegativeIntNumberInFilter_NoException()
        {
            // Arrange
            var query = "$filter=Pin eq -1";

            // Act
            Action call = () => SelectOperation.Parse(query);

            // Assert
            call.Should().NotThrow();
        }

        /// <summary>
        /// IADFRAME-1011 OData-парсер не работает с отрицательными значениями фильтров
        /// </summary>
        [TestCase("m")]
        [TestCase("M")]
        public void SelectOperation_ParseNegativeDecimalNumberInFilter_NoException(string m)
        {
            // Arrange
            var query = "$filter=Pin eq -1" + m;

            // Act
            Action call = () => SelectOperation.Parse(query);

            // Assert
            call.Should().NotThrow();
        }

        [Test]
        public void SelectOperation_ParsePositiveNumberInFilter_NoException()
        {
            // Arrange
            var query = "$filter=Pin eq 1";

            // Act
            Action call = () => SelectOperation.Parse(query);

            // Assert
            call.Should().NotThrow();
        }

        private void Test(string parsingString, SelectOperation expectedOperation)
        {
            if (parsingString == null) throw new ArgumentNullException(nameof(parsingString));
            if (expectedOperation == null) throw new ArgumentNullException(nameof(expectedOperation));

            var parsedSelectOperation = SelectOperation.Parse(parsingString);

            var equalsResult = parsedSelectOperation.Equals(expectedOperation);

            Debug.Assert(equalsResult);
        }
    }
}
