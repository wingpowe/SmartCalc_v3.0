.PHONY : all clean tests gcov_report leaks cpplint cppcheck requirements

CPP = g++ -std=c++17 -lstdc++
FLAGS = -pedantic-errors -Wall -Werror -Wextra
COV_FALGS = -fprofile-arcs -ftest-coverage

OS= $(shell uname)

ifeq ($(OS), Linux)
	OPEN = xdg-open
	LIBS = -lgtest -pthread --coverage
	LEAKS = valgrind --track-origins=yes --leak-check=full  ./s21_test
endif

ifeq ($(OS), Darwin)
	OPEN = open
	LIBS = -lgtest -L /usr/local/lib --coverage
	LEAKS = leaks --atExit -- ./s21_test
endif


all: build

build: clean
	$(CPP) -shared -o libModel_calc.so $(FLAGS) -fPIC Model_calc.cc

clean:
	@rm -f *.o *.a *.gcno *.gcda *.info *.out *.gcov *.html
	@rm -rf ./report ./html ./latex ./build
	@rm -f GCOV_rep s21_test
	@rm -f texinfo.aux texinfo.cp texinfo.cps texinfo.dvi texinfo.log texinfo.toc

tests: clean
	$(CPP)  test.cc Model_calc.cc -g $(LIBS) $(FLAGS) $(COV_FALGS) -o s21_test
	./s21_test

gcov_report: clean test 
	lcov --no-external -t "test_calc" -o test_calc.info -c -d .
	genhtml -o report test_calc.info
	@$(OPEN) ./report/index.html

leaks: tests
	$(LEAKS) 

cpplint:
	# @clang-format -style=Google -i *.cc *.h
	clang-format -style=Google -n *.cc *.h

cppcheck:
	@cppcheck --language=c++ --enable=all --suppress=missingIncludeSystem Model_calc.cc test.cc Model_calc.h

requirements:
	sudo apt install -y g++ valgrind libgtest-dev clang-format gcovr lcov cppcheck
