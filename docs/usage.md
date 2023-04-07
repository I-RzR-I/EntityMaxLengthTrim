# USING

For correctly using this repository you must use one of the  following property attributes:
<br /> => `MaxLengthAttribute` -> ([MaxLength(**x**)]);
<br /> or
<br /> => `StringLengthAttribute` -> ([StringLength(**x**)]);
<br /> or
<br /> => `MaxAllowedLengthAttribute` -> ([MaxAllowedLength(**x**)]).

The information about `MaxLengthAttribute` and `StringLengthAttribute` are located in `System.ComponentModel.DataAnnotations`.
In the current repository was added a new property attribute `MaxAllowedLengthAttribute` where you must specify the maximum allowed string length. 

This attribute has not applied any validation and is the extension for the `Attribute` class.

In addition for more options in use, was added a few settings:
```csharp
TEntity ApplyStringMaxAllowedLength<TEntity>(TEntity entity, bool useDotOnEnd = false);
TEntity ApplyStringMaxAllowedLength<TEntity>(TEntity entity, IReadOnlyCollection<string> truncateWithDots, bool processOnlyAssigned = false);
TEntity ApplyStringMaxAllowedLength<TEntity>(TEntity entity, string propertyName, bool useDots = true);
TEntity ApplyStringMaxAllowedLength<TEntity>(TEntity entity, IReadOnlyCollection<PropertyOption> options, bool processOnlyAssigned = false);
```

##### Data model:
```csharp
public class Foo
   {
       public int Id { get; set; }

       public DateTime CreatedOn { get; set; }

       [MaxLength(20)]
       public string Name { get; set; }

       [StringLength(50)]
       public string FullName { get; set; }

       [MaxAllowedLength(100)]
       public string Description { get; set; }
   }
```

##### Usage of truncate all strings at the maximum allowed length
```csharp
var newParsedData = StringInterceptor.ApplyStringMaxAllowedLength(new Foo()
    {
        Name = "Here should be the new name of FOO",
        CreatedOn = DateTime.MaxValue,
        Id = 1,
        FullName = string.Empty,
        Description = null
    });
var newParsedDataWithDots = StringInterceptor.ApplyStringMaxAllowedLength(new Foo()
    {
        Name = "Here should be the new name of FOO",
        CreatedOn = DateTime.MaxValue,
        Id = 1,
        FullName = string.Empty,
        Description = null
    }, true);

// Results, length 20 characters:
// newParsedData.Name => 'Here should be the n'
// newParsedDataWithDots.Name => 'Here should be th...'

```

```csharp
public class Foo : INotifyPropertyChanged
{
    private string _name;
    private string _fullName;
    private string _description;

    [MaxLength(30)]
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    [StringLength(50)]
    public string FullName
    {
        get => _fullName;
        set
        {
            if (_fullName == value) return;
            _fullName = value;
            OnPropertyChanged(nameof(FullName));
        }
    }

    [MaxAllowedLength(150)]
    public string Description
    {
        get => _description;
        set
        {
            if (_description == value) return;
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        StringInterceptor.ApplyStringMaxAllowedLength(this, propertyName, false);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```


