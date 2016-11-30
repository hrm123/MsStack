var app = angular.module('ContactMgmtApp');
app.directive('kendoMultiselectDctv', function () {
    return {
        scope: {
            kDataSource: '=',//the data source for the directive
            onSelect: '&',//callback function for items selected change
            kDataItem: '=',//pre-selected or current items data bound both ways
        },
        link: function (scope, element, attrs) {
            var exisitingItems = [];
            if (typeof (scope.kDataItem) != "undefined") {
                exisitingItems = scope.kDataItem;
            }
            scope.$watch("kDataSource", function (v) {//rebind the multiselect whenever the datasource changes
                element.kendoMultiSelect({
                    dataTextField: attrs.kDataTextField,
                    dataValueField: attrs.kDataValueField,
                    dataSource: scope.kDataSource,
                    placeholder: attrs.defaultText,
                    value: exisitingItems,
                    change: function (e) {
                        var currentItems = e.sender._values;
                        debugger;
                        if (typeof scope.kDataItem != "undefined") { //update selected item on parent scope
                            scope.$apply(function () {
                                scope.kDataItem = currentItems;
                            });
                        }
                        scope.onSelect({ items: currentItems });
                    }
                });
            }, true);
        }
    };
});