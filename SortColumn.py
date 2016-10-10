'''
SortColumn.vb
This file appears to be a custom sort class, which similarly 
might be required by PyQt4. See:
https://stackoverflow.com/questions/22489018/pyqt-how-to-get-most-of-qlistwidget

---
Sorting is alphabetical and case-insensitive by default. If you want a 
custom sort-order, subclass QListWidgetItem and re-implement its less-than 
operator:

class ListWidgetItem(QtGui.QListWidgetItem):
    def __lt__(self, other):
        return self.text() < other.text() # or whatever

class MyTableWidgetItem(QtGui.QTableWidgetItem):
    def __lt__(self, other):
        return self.sortKey < other.sortKey # or whatever
---

The original implementation first tries to compare the column values
by coercing them to numbers, if that fails it tries to coerce them to
dates, then if that fails it tries to coerce them to strings.
'''