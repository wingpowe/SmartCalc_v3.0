
#ifndef CPP3_SMARTCALC_V2_STRC_MODEL_CALC_H
#define CPP3_SMARTCALC_V2_STRC_MODEL_CALC_H

#include <algorithm>
#include <cmath>
#include <functional>
#include <iostream>
#include <regex>
#include <set>
#include <sstream>
#include <string>
#include <vector>

#ifdef __cplusplus
extern "C" {
#endif

namespace s21 {

class ModelCalc {
 public:
  ModelCalc() : noerror_(false), rezult_("empty expression"){};
  ModelCalc(const std::string &str, double x) : noerror_(false) {
    Parsing(str, x);
  }
  bool Parsing(std::string str, double x);
  double Calculate(double x);
  std::string rezult() { return rezult_; }

 private:
  using Stack = std::vector<std::pair<int, double>>;
  Stack stack_;
  bool noerror_;
  std::string rezult_;
  bool Pars(std::string line);
  bool ToPolish();
};

class Date {
 public:
  Date() : day_(0), month_(0), year_(0){};
  explicit Date(const std::string &str) { StrToDate(str); }
  void AddMonth();
  void StrToDate(std::string str);
  std::string DateToStr();
  int day() { return day_; }

 private:
  unsigned int day_, month_, year_;
};

class CreditCalc {
 public:
  CreditCalc() : overpayment_(0.), total_payment_(0.), count_payment_(0){};
  void credit_calc(double amount, double interest, int type, int months,
                   int years, std::string sdate);
  std::string &date(int index) { return date_[index]; }
  const double &payment(int index) { return payment_[index]; }
  const double &rate(int index) { return rate_[index]; }
  const double &body(int index) { return body_[index]; }
  const double &remainder(int index) { return remainder_[index]; }
  const double &overpayment() { return overpayment_; }
  const double &total_payment() { return total_payment_; }
  const int &count_payment() { return count_payment_; }

 private:
  std::vector<std::string> date_;
  std::vector<double> payment_, rate_, body_, remainder_;
  double overpayment_, total_payment_;
  int count_payment_;
  void different(double interest, Date date_pay);
  void annuity(double interest, Date date_pay);
};

}  // namespace s21

// Экспортируемая функция
s21::ModelCalc *create_calculator();
void destroy_calculator(s21::ModelCalc *calculator);
bool parsing(s21::ModelCalc *calculator, const char *expression,
             const double x);
double calculate(s21::ModelCalc *calculator, const double x);
const char *result(s21::ModelCalc *calculator);

// Экспортируемые функции для CreditCalc
s21::CreditCalc* create_credit_calculator();
void destroy_credit_calculator(s21::CreditCalc* calculator);
void credit_calc(s21::CreditCalc* calculator, double amount, double interest, int type, int months, int years, const char* sdate);
const char* credit_date(s21::CreditCalc* calculator, int index);
double credit_payment(s21::CreditCalc* calculator, int index);
double credit_rate(s21::CreditCalc* calculator, int index);
double credit_body(s21::CreditCalc* calculator, int index);
double credit_remainder(s21::CreditCalc* calculator, int index);
double credit_overpayment(s21::CreditCalc* calculator);
double credit_total_payment(s21::CreditCalc* calculator);
int credit_count_payment(s21::CreditCalc* calculator);


#ifdef __cplusplus
}
#endif

#endif  // CPP3_SMARTCALC_V2_STRC_MODEL_CALC_H