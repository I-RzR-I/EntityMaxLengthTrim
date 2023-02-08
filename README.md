> **Note** This repository is developed for .netstandard1.5+ and net framework 4.5

[![NuGet Version](https://img.shields.io/nuget/v/EntityMaxLengthTrim.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/EntityMaxLengthTrim/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/EntityMaxLengthTrim.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/EntityMaxLengthTrim)

One important thing about this repository is that you can truncate input string in the fields/properties at the maximum allowed length from DB. To specify the maximum allowed string length you can use data annotation attributes predefined in `System.ComponentModel.DataAnnotations` or a new custom attribute. 

The maximum allowed length will be searched in the attributes(you may use one of these):
* `MaxLengthAttribute` -> ([MaxLength(**x**)]);
* `StringLengthAttribute` -> ([StringLength(**x**)]);
* `MaxAllowedLengthAttribute` -> ([MaxAllowedLength(**x**)]).

> To get acquainted with a more detailed description, please check the content table at [the first point](docs/usage.md).

Once you use this repository, you have the possibility to avoid database exceptions related to exceeding the limit of the maximum allowed length and the string type columns.

No additional components or packs are required for use. So, it only needs to be added/installed in the project and can be used instantly.

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/EntityMaxLengthTrim" target="_blank">nuget.org</a>** or specify what version you want:


> `Install-Package EntityMaxLengthTrim -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
1. [CHANGELOG](docs/CHANGELOG.md)
1. [BRANCH-GUIDE](docs/branch-guide.md)