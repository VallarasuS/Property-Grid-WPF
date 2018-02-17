# Property-Grid-WPF
A winforms like Property Grid implementation for WPF

## Usage

``` xaml

    <Grid x:Name="theGrid">
        <vasu:PropertyGrid SelectedObject="{Binding ElementName=theGrid}"/>
    </Grid>

```

## Features
  * Data binding support
  * Works with MVVM, PRSIM Patterns
  * Sorting & Grouping
  * Property Name and Description view
  
  ![WPF Property Grid](/images/PropertyGrid.PNG)
