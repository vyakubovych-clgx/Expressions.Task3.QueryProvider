using Expressions.ComplexTask.LinqProvider.Entities;
using Expressions.ComplexTask.LinqProvider.Provider;
using Microsoft.Extensions.Configuration;

namespace Expressions.ComplexTask.Tests;

[TestFixture]
public class IntegrationTests
{
    private EntitySet<Catalogue> _catalogues;
    private EntitySet<Material> _materials;
    private EntitySet<Assessment> _assessments;

    [SetUp]
    public void SetUp()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var connectionString = configuration.GetConnectionString("CardFile");

        _catalogues = new EntitySet<Catalogue>(connectionString);
        _materials = new EntitySet<Material>(connectionString);
        _assessments = new EntitySet<Assessment>(connectionString);
    }

    [Test]
    public void EntitySet_ShouldReturnAllCatalogues()
    {
        var actualCatalogues = _catalogues.ToList();
        Assert.That(actualCatalogues, Has.Count.EqualTo(10));
    }

    [Test]
    public void EntitySet_ShouldReturnMaterialsByCatalogueId()
    {
        var expectedMaterialIds = new[] {50, 54};
        var actualMaterials = _materials.Where(m => m.CatalogueId == 44).ToList();
        Assert.That(expectedMaterialIds, Is.EquivalentTo(actualMaterials.Select(m => m.Id)));
    }

    [Test]
    public void EntitySet_ShouldReturnMaterialByTitle()
    {
        var expectedMaterials = new[]
        {
            new Material
            {
                Id = 48,
                Title = "Lorem ipsum",
                Text =
                    "Lorem ipsum dolor sit aot, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas sed sed risus pretium quam vulputate dignissim suspendisse. Arcu dictum varius duis at consectetur. Viverra nam libero justo laoreet sit. Volutpat ac tincidunt vitae semper quis lectus nulla. Gravida arcu ac tortor dignissim. Sed odio morbi quis commodo odio. Venenatis cras sed felis eget velit aliquet sagittis id consectetur. Auctor elit sed vulputate mi sit amet mauris commodo. Diam ut venenatis tellus in metus vulputate eu scelerisque. Integer vitae justo eget magna. Arcu cursus euismod quis viverra nibh. Urna id volutpat lacus laoreet non. Elementum nibh tellus molestie nunc.\n\nSapien nec sagittis aliquam malesuada bibendum arcu. Sed pulvinar proin gravida hendrerit lectus. A condimentum vitae sapien pellentesque habitant morbi. Nibh cras pulvinar mattis nunc sed. Id diam maecenas ultricies mi eget mauris pharetra et ultrices. Aliquam nulla facilisi cras fermentum odio. Vel turpis nunc eget lorem dolor sed viverra. Fermentum iaculis eu non diam phasellus vestibulum lorem sed risus. Habitasse platea dictumst quisque sagittis purus sit amet volutpat. Diam donec adipiscing tristique risus nec feugiat in. Sed elementum tempus egestas sed. Congue eu consequat ac felis donec et odio pellentesque. At volutpat diam ut venenatis tellus in. Arcu odio ut sem nulla pharetra. Mauris sit amet massa vitae tortor. Egestas sed sed risus pretium.\n\nEt netus et malesuada fames ac turpis. Sit amet volutpat consequat mauris nunc congue nisi vitae suscipit. Porttitor eget dolor morbi non arcu risus quis varius. Ornare lectus sit amet est placerat in egestas. Aliquet lectus proin nibh nisl. Vel quam elementum pulvinar etiam. Nibh venenatis cras sed felis eget velit aliquet. Mattis rhoncus urna neque viverra justo. Massa ultricies mi quis hendrerit dolor magna eget. Sodales neque sodales ut etiam sit amet nisl purus in. Fermentum odio eu feugiat pretium nibh. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Donec enim diam vulputate ut. Amet massa vitae tortor condimentum. Vitae purus faucibus ornare suspendisse. Eleifend donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Sed felis eget velit aliquet.\n\nPorta non pulvinar neque laoreet. Posuere morbi leo urna molestie at elementum eu. Facilisis sed odio morbi quis. Diam maecenas ultricies mi eget mauris pharetra et ultrices. Fusce id velit ut tortor. Volutpat est velit egestas dui id ornare arcu. Blandit aliquam etiam erat velit scelerisque in. Eget mi proin sed libero enim sed faucibus. Tincidunt tortor aliquam nulla facilisi cras fermentum odio eu feugiat. Amet consectetur adipiscing elit duis. Euismod elementum nisi quis eleifend quam. In hac habitasse platea dictumst. Elementum sagittis vitae et leo.\n\nVitae turpis massa sed elementum. Id eu nisl nunc mi ipsum. Nunc sed blandit libero volutpat sed cras ornare arcu dui. Maecenas volutpat blandit aliquam etiam erat velit scelerisque in. Risus commodo viverra maecenas accumsan lacus vel facilisis volutpat est. Sit amet mauris commodo quis imperdiet massa tincidunt. Arcu non odio euismod lacinia at quis risus. Nunc mi ipsum faucibus vitae. Ornare suspendisse sed nisi lacus sed viverra tellus in. Sem viverra aliquet eget sit amet tellus. Sed adipiscing diam donec adipiscing tristique risus nec feugiat. Urna duis convallis convallis tellus. Iaculis eu non diam phasellus vestibulum lorem sed risus.",
                CatalogueId = 41
            }
        };

        var actualMaterials = _materials.Where(m => m.Title == "Lorem ipsum").ToList();
        Assert.That(actualMaterials, Has.Count.EqualTo(1));
        Assert.That(actualMaterials[0], Is.EqualTo(expectedMaterials[0]));
    }

    [Test]
    public void EntitySet_ShouldReturnMaterialsWithDifferentCatalogueId()
    {
        var expectedMaterialIds = new[] { 45, 46, 49, 50, 54 };
        var actualMaterials = _materials.Where(m => m.CatalogueId != 41).ToList();
        Assert.That(expectedMaterialIds, Is.EquivalentTo(actualMaterials.Select(m => m.Id)));
    }

    [Test]
    public void EntitySet_ShouldReturnAllAssessmentsWithRatingGreaterThan4()
    {
        var expectedAssessmentIds = new[] { 25, 26, 27 };
        var actualAssessments = _assessments.Where(a => a.Rating > 4).ToList();
        Assert.That(expectedAssessmentIds, Is.EquivalentTo(actualAssessments.Select(a => a.Id)));
    }

    [Test]
    public void EntitySet_ShouldReturnAllAssessmentsWithRatingGreaterThanOrEqual4()
    {
        var expectedAssessmentIds = new[] { 25, 26, 27, 28 };
        var actualAssessments = _assessments.Where(a => a.Rating >= 4).ToList();
        Assert.That(expectedAssessmentIds, Is.EquivalentTo(actualAssessments.Select(a => a.Id)));
    }

    [Test]
    public void EntitySet_ShouldReturnAllAssessmentsWithRatingLessThan5()
    {
        var expectedAssessmentIds = new[] { 28, 29 };
        var actualAssessments = _assessments.Where(a => a.Rating < 5).ToList();
        Assert.That(expectedAssessmentIds, Is.EquivalentTo(actualAssessments.Select(a => a.Id)));
    }

    [Test]
    public void EntitySet_ShouldReturnAllAssessmentsWithRatingLessThanOrEqual5()
    {
        var expectedAssessmentIds = new[] {25, 26, 27, 28, 29};
        var actualAssessments = _assessments.Where(a => a.Rating <= 5).ToList();
        Assert.That(expectedAssessmentIds, Is.EquivalentTo(actualAssessments.Select(a => a.Id)));
    }

    [Test]
    public void EntitySet_ShouldReturnMaterialsByCatalogueIdWithDifferentArgumentOrder()
    {
        var expectedMaterialIds = new[] { 50, 54 };
        var actualMaterials = _materials.Where(m => 44 == m.CatalogueId).ToList();
        Assert.That(expectedMaterialIds, Is.EquivalentTo(actualMaterials.Select(m => m.Id)));
    }

    [Test]
    public void EntitySet_ShouldReturnAssessmentByRatingAndMaterialId()
    {
        var expectedAssessments = new[]
        {
            new Assessment
            {
                Id = 25,
                MaterialId = 48,
                Rating = 5
            }
        };

        var actualAssessments = _assessments.Where(a => a.Rating == 5 && a.MaterialId == 48).ToList();
        Assert.That(actualAssessments, Has.Count.EqualTo(1));
        Assert.That(actualAssessments[0], Is.EqualTo(expectedAssessments[0]));
    }
}