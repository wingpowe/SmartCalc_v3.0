.PHONY : all install uninstall clean dist tests

all: tests


install: dist
	dpkg -i smartcalc.deb
	apt-get install -f

uninstall: 
	dpkg -r smartcalc

clean:
	rm -rf smartcalc SmartCalc.Core/bin SmartCalc.Core/obj SmartCalc.Core/libModel_calc.so SmartCalc.Gui/bin SmartCalc.Gui/obj SmartCalc.Gui/history.json SmartCalc.Gui/logs SmartCalc.Core.Tests/bin SmartCalc.Core.Tests/obj SmartCalc.app install.sh uninstall.sh

tests:
	dotnet test 

dist: 
	./buildDotDeb.sh 


