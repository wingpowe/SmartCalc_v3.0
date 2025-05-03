#include <gtest/gtest.h>

#include <cmath>

#include "Model_calc.h"

TEST(test_add, test1) {
  s21::ModelCalc calc("1+1", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), 1.0 + 1.0);
  calc.Parsing("0.00001 + 1e-6", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), 0.00001 + 1e-6);
}

TEST(test_sub, test1) {
  s21::ModelCalc calc("1-1", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), 1.0 - 1.0);
  calc.Parsing("0.00001 - 1e-6", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), 0.00001 - 1e-6);
}

TEST(test_mult, test1) {
  s21::ModelCalc calc("53*17", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), 53. * 17.);
  calc.Parsing("0.0005 * 3e-6", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), 0.0005 * 3e-6);
}

TEST(test_div, test1) {
  s21::ModelCalc calc("0.00015 / 3e-6", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), 0.00015 / 3e-6);
  calc.Parsing("10 / 3", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), 10. / 3.);
}

TEST(test_mod, test1) {
  s21::ModelCalc calc("10 mod 3", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), fmod(10., 3.));
}

TEST(test_pow, test1) {
  s21::ModelCalc calc("2 ^ 2 ^ 3", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), pow(2., pow(2., 3.)));
  calc.Parsing("5 ^ 2e-5", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), pow(5., 2e-5));
}

TEST(test_unar_minus, test1) {
  s21::ModelCalc calc("-5", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), -5.);
  calc.Parsing("-(-(-5))", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), -(-(-5)));
}

TEST(test_unar_plus1, test1) {
  s21::ModelCalc calc("+5", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), +5.);
  calc.Parsing("+(+(+5))", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), +(+(+5)));
}

TEST(test_persent1, test1) {
  s21::ModelCalc calc("50%", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), 50. / 100.);
}

TEST(test_sin, test1) {
  s21::ModelCalc calc("sin(2)", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), sin(2.));
}

TEST(test_x, test1) {
  s21::ModelCalc calc("55+x", 5.0);
  ASSERT_EQ(calc.Calculate(5.0), 55. + 5.);
}

TEST(test_error1, test1) {
  s21::ModelCalc calc("55+x-soe", 5.0);
  EXPECT_TRUE(calc.rezult() == "invalid characters used");
}

TEST(test_error2, test1) {
  s21::ModelCalc calc("55+x(", 5.0);
  EXPECT_TRUE(calc.rezult() == "incorrect placement of parentheses");
}

TEST(test_error3, test1) {
  s21::ModelCalc calc("55+x+", 5.0);
  EXPECT_TRUE(calc.rezult() == "wrong number operators");
}

TEST(test_error4, test1) {
  s21::ModelCalc calc("", 5.0);
  EXPECT_TRUE(calc.rezult() == "empty expression");
}

TEST(test_error5, test1) {
  std::string str;
  for (int i = 0; i < 259; i++) str.push_back('x');
  s21::ModelCalc calc(str, 5.0);
  EXPECT_TRUE(calc.rezult() == "too long expression");
}

TEST(test_cos, test1) {
  s21::ModelCalc calc("cos(2)", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), cos(2.));
}

TEST(test_tan1, test1) {
  s21::ModelCalc calc("tan(2)", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), tan(2.));
}

TEST(test_atan1, test1) {
  s21::ModelCalc calc("atan(2)", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), atan(2.));
}

TEST(test_acos, test1) {
  s21::ModelCalc calc("acos(1)", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), acos(1.));
}

TEST(test_asin, test1) {
  s21::ModelCalc calc("asin(1)", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), asin(1.));
}

TEST(test_sqrt, test1) {
  s21::ModelCalc calc("sqrt(4)", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), sqrt(4.));
}

TEST(test_log, test1) {
  s21::ModelCalc calc("log(100)", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), log(100.));
}

TEST(test_ln, test1) {
  s21::ModelCalc calc("ln(100)", 0.0);
  ASSERT_EQ(calc.Calculate(0.0), log10(100.));
}

TEST(test_credit1, test1) {
  std::string date_pay{"29.02.2020"};
  s21::CreditCalc credit;
  credit.credit_calc(1000000.0, 6.0, 2, 12, 0, date_pay);
  ASSERT_EQ(round(credit.overpayment()), round(32797));
}

TEST(test_credit2, test1) {
  std::string date_pay{"29.02.2020"};
  s21::CreditCalc credit;
  credit.credit_calc(1000000.0, 6.0, 1, 12, 0, date_pay);
  ASSERT_EQ(round(credit.overpayment()), round(32500));
}

int main(int argc, char *argv[]) {
  ::testing::InitGoogleTest(&argc, argv);
  return RUN_ALL_TESTS();
}