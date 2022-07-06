using System.Linq.Expressions;
using Expressions.ComplexTask.LinqProvider.Entities;
using Expressions.ComplexTask.LinqProvider.Provider;

namespace Expressions.ComplexTask.Tests;

[TestFixture]
public class TranslatorTests
{
    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateSimpleSelect()
    {
        var expression = Expression.Constant(new EntitySet<Catalogue>(string.Empty));
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Catalogues"));
    }

    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateWhereClauseWithEqualityConditionWithNumber()
    {
        Expression<Func<IQueryable<Material>, IQueryable<Material>>> expression =
            materials => materials.Where(m => m.CatalogueId == 41);
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Materials WHERE CatalogueId = 41"));
    }

    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateWhereClauseWithEqualityConditionWithString()
    {
        Expression<Func<IQueryable<Material>, IQueryable<Material>>> expression =
            materials => materials.Where(m => m.Title == "Lorem ipsum");
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Materials WHERE Title = 'Lorem ipsum'"));
    }

    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateWhereClauseWithNotEqualityCondition()
    {
        Expression<Func<IQueryable<Material>, IQueryable<Material>>> expression =
            materials => materials.Where(m => m.CatalogueId != 41);
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Materials WHERE CatalogueId <> 41"));
    }

    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateWhereClauseWithGreaterThanCondition()
    {
        Expression<Func<IQueryable<Assessment>, IQueryable<Assessment>>> expression =
            assessments => assessments.Where(a => a.Rating > 4);
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Assessments WHERE Rating > 4"));
    }

    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateWhereClauseWithGreaterThanOrEqualCondition()
    {
        Expression<Func<IQueryable<Assessment>, IQueryable<Assessment>>> expression =
            assessments => assessments.Where(a => a.Rating >= 4);
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Assessments WHERE Rating >= 4"));
    }

    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateWhereClauseWithLessThanCondition()
    {
        Expression<Func<IQueryable<Assessment>, IQueryable<Assessment>>> expression =
            assessments => assessments.Where(a => a.Rating < 5);
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Assessments WHERE Rating < 5"));
    }

    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateWhereClauseWithLessThanOrEqualCondition()
    {
        Expression<Func<IQueryable<Assessment>, IQueryable<Assessment>>> expression =
            assessments => assessments.Where(a => a.Rating <= 5);
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Assessments WHERE Rating <= 5"));
    }

    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateWhereClauseWithDifferentArgumentOrder()
    {
        Expression<Func<IQueryable<Material>, IQueryable<Material>>> expression =
            materials => materials.Where(m => 41 == m.CatalogueId);
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Materials WHERE 41 = CatalogueId"));
    }

    [Test]
    public void ExpressionToSqlTranslator_ShouldTranslateWhereClauseWithAnd()
    {
        Expression<Func<IQueryable<Assessment>, IQueryable<Assessment>>> expression =
            assessments => assessments.Where(a => a.Rating == 5 && a.MaterialId == 48);
        var translator = new ExpressionToSqlTranslator();
        var result = translator.Translate(expression);
        Assert.That(result, Is.EqualTo("SELECT * FROM Assessments WHERE Rating = 5 AND MaterialId = 48"));
    }
}