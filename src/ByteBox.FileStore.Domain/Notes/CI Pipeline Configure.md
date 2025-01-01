### CI Pipeline Configuration

- Added following workflow which will be trigerred when someone create a pull request or push on main branch.
- It will checkout to the branch, setup .NET on machine executing the pipeline, restore dependencies, build and run tests.
- Also, created a branch protection rules so that no one can push code directly into the main branch by enabling `Require a pull request before merging`. A pull request will be required to push changes into main branch. And enabled `Require status checks to pass before merging` and `Require branches to be up to date before merging` and added the test pipeline in `Status check that are required` list so that a pull request can't be merged before successfully passing tests.


```

name: .NET CI Pipeline

on:
  push:
    branches: [ "main" ]
  pull_request:
    branches: [ "main" ]

jobs:
  build:
    name: Build and Test
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v4
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 8.0.x
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --no-restore
    - name: Test
      run: dotnet test --no-build --verbosity normal

```