#include "Model_calc.h"

bool s21::ModelCalc::Pars(std::string line) {
  std::vector<std::string> operations{
      "\\+",  "\\-",
      "\\*",  "\\/",
      "mod",  "\\^",
      "sin",  "cos",
      "tan",  "acos",
      "asin", "atan",
      "sqrt", "ln",
      "log",  "%",
      "\\+",  "\\-",
      "\\(",  "\\)",
      "[xX]", "[-+]?[0-9]*[.,]?[0-9]+(?:[eE][-+]?[0-9]+)?"};
  bool mach{true};
  std::regex rxp;
  std::smatch rgx_sm;
  while (!line.empty() && mach) {
    mach = false;
    for (int i = 0; i < 22; i++) {
      rxp = "^" + operations[i];
      if (std::regex_search(line, rgx_sm, rxp)) {
        stack_.push_back(
            std::make_pair(i, (i == 21) ? std::stod(rgx_sm.str()) : 0.0));
        line = rgx_sm.suffix();
        mach = true;
        break;
      }
    }
  }
  if (mach) {
    if (stack_[0].first == 0 || stack_[0].first == 1) stack_[0].first += 16;
    std::set<int> o_set{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 18};
    for (std::size_t i = 1; i < stack_.size(); i++) {
      if ((o_set.find(stack_[i - 1].first) != o_set.end()) &&
          (stack_[i].first == 0 || stack_[i].first == 1))
        stack_[i].first += 16;
    }
    std::reverse(stack_.begin(), stack_.end());
  }
  return mach;
}

bool s21::ModelCalc::ToPolish() {
  int sw[20]{1, 1, 2, 2, 2, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 5, 5, 5, 6, 7};
  int getAction[7][8]{{4, 1, 1, 1, 1, 1, 1, 5}, {2, 2, 1, 1, 1, 1, 1, 2},
                      {2, 2, 2, 1, 1, 1, 1, 2}, {2, 2, 2, 2, 1, 1, 1, 2},
                      {2, 2, 2, 2, 1, 1, 1, 2}, {2, 2, 2, 2, 2, 2, 1, 2},
                      {5, 1, 1, 1, 1, 1, 1, 3}};
  bool noerror{true};
  Stack first, second;
  while (true) {
    if (!stack_.empty() &&
        ((stack_.back().first == 20) || (stack_.back().first == 21))) {
      second.push_back(stack_.back());
      stack_.pop_back();
    } else {
      int n{getAction[(first.empty()) ? 0 : sw[first.back().first]]
                     [(stack_.empty()) ? 0 : sw[stack_.back().first]]};
      if (n == 1) {
        first.push_back(stack_.back());
        stack_.pop_back();
      } else if (n == 2) {
        second.push_back(first.back());
        first.pop_back();
      } else if (n == 3) {
        stack_.pop_back();
        first.pop_back();
      } else if (n == 4) {
        break;
      } else if (n == 5) {
        noerror = false;
        break;
      }
    }
  }
  if (noerror) {
    std::reverse(second.begin(), second.end());
    std::swap(second, stack_);
  }
  return noerror;
}

double s21::ModelCalc::Calculate(double x) {
     std::vector<std::function<double(double, double)>> bin_func{
      [](double a, double b) { return a + b; },
      [](double a, double b) { return a - b; },
      [](double a, double b) { return a * b; },
      [](double a, double b) { return a / b; },
#ifdef __APPLE__
      [](double a, double b) { return std::fmod(a, b); },
      [](double a, double b) { return std::pow(a, b); }
#else
      fmod,
      pow
#endif
  };

  std::vector<std::function<double(double)>> unar_func{
#ifdef __APPLE__
      [](double a) { return std::sin(a); },
      [](double a) { return std::cos(a); },
      [](double a) { return std::tan(a); },
      [](double a) { return std::acos(a); },
      [](double a) { return std::asin(a); },
      [](double a) { return std::atan(a); },
      [](double a) { return std::sqrt(a); },
      [](double a) { return std::log10(a); },
      [](double a) { return std::log(a); },
#else
      sin,
      cos,
      tan,
      acos,
      asin,
      atan,
      sqrt,
      log10,
      log,
#endif
      [](double a) { return a / 100; },
      [](double a) { return a; },
      [](double a) { return -a; }
  };
  Stack rez, stack(stack_);
  while (!stack_.empty() && noerror_) {
    if (stack_.back().first == 20) {
      rez.push_back(stack_.back());
      rez.back().second = x;
      stack_.pop_back();
    } else if (stack_.back().first == 21) {
      rez.push_back(stack_.back());
      stack_.pop_back();
    } else {
      if (stack_.back().first < 6 && rez.size() > 1) {
        double tmp = rez.back().second;
        rez.pop_back();
        rez.back().second =
            bin_func[stack_.back().first](rez.back().second, tmp);
        stack_.pop_back();
      } else if (stack_.back().first < 18 && stack_.back().first > 5 &&
                 !rez.empty()) {
        rez.back().second =
            unar_func[stack_.back().first - 6](rez.back().second);
        stack_.pop_back();
      } else {
        noerror_ = false;
        break;
      }
    }
  }
  std::swap(stack, stack_);
  noerror_ = (rez.size() > 1) ? false : noerror_;
  return (noerror_) ? rez.back().second : 0.0;
}

bool s21::ModelCalc::Parsing(std::string str, double x) {
  str.erase(std::remove(str.begin(), str.end(), ' '), str.end());
  int len = str.length();
  noerror_ = false;
  stack_.clear();
  if (len == 0) {
    rezult_ = "empty expression";
  } else if (len > 256) {
    rezult_ = "too long expression";
  } else if (!this->Pars(str)) {
    rezult_ = "invalid characters used";
  } else if (!this->ToPolish()) {
    rezult_ = "incorrect placement of parentheses";
  } else {
    noerror_ = true;
    double rez = this->Calculate(x);
    if (noerror_) {
      std::stringstream strm1;
      strm1 << rez;
      strm1 >> rezult_;
    } else {
      rezult_ = "wrong number operators";
    }
    while (rezult_.back() == '0') rezult_.pop_back();
    if (rezult_.back() == '.') rezult_.pop_back();
  }
  return noerror_;
}

void s21::Date::AddMonth() {
  if (month_ < 12)
    month_++;
  else
    month_ = 1, year_++;
  unsigned days[12] = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
  for (unsigned i = 2000; i <= year_; i += 4)
    if (i == year_) days[1]++;
  day_ = (day_ <= days[month_ - 1]) ? day_ : days[month_ - 1];
}

void s21::Date::StrToDate(std::string str) {
  day_ = std::stoi(str.substr(0, 2));
  month_ = std::stoi(str.substr(3, 2));
  year_ = std::stoi(str.substr(6, 4));
}

std::string s21::Date::DateToStr() {
  std::string day =
      (day_ < 10) ? "0" + std::to_string(day_) : std::to_string(day_);
  std::string month =
      (month_ < 10) ? "0" + std::to_string(month_) : std::to_string(month_);
  return (day + "." + month + "." + std::to_string(year_));
}

void s21::CreditCalc::different(double interest, Date date_pay) {
  double persent_month = (interest / 100.) / 12.;
  double monthly_pay = round((remainder_[0] / count_payment_) * 100.) / 100.;
  for (int i = 1; i <= count_payment_; i++) {
    rate_.push_back(round(persent_month * remainder_.back() * 100) / 100);
    payment_.push_back(monthly_pay + rate_.back());
    body_.push_back(monthly_pay);
    date_pay.AddMonth();
    date_.push_back(date_pay.DateToStr());
    remainder_.push_back(remainder_.back() - monthly_pay);
  }
  if (count_payment_ > 1 && body_.back() != remainder_[remainder_.size() - 2]) {
    body_.back() = remainder_[remainder_.size() - 2];
    payment_.back() = rate_.back() + body_.back();
    remainder_.back() = remainder_[remainder_.size() - 2] - body_.back();
  };
}

void s21::CreditCalc::credit_calc(double amount, double interest, int type,
                                  int months, int years, std::string sdate) {
  using Date = s21::Date;
  Date date_pay(sdate);
  count_payment_ = years * 12 + months;
  date_.push_back(sdate);
  remainder_.push_back(amount);
  payment_.push_back(0.), rate_.push_back(0.), body_.push_back(0.);
  if (type == 1) {
    different(interest, date_pay);
  } else if (type == 2) {
    annuity(interest, date_pay);
  }
  for (int i = 1; i <= count_payment_; i++) total_payment_ += payment_[i];
  overpayment_ = total_payment_ - remainder_[0];
}

void s21::CreditCalc::annuity(double interest, Date date_pay) {
  double persent_month = (interest / 100.) / 12.;
  double monthly_pay =
      round(
          remainder_.back() *
          (persent_month +
           (persent_month / (pow((1. + persent_month), count_payment_) - 1))) *
          100) /
      100;
  for (int i = 1; i <= count_payment_; i++) {
    rate_.push_back(round(persent_month * remainder_.back() * 100) / 100);
    payment_.push_back(monthly_pay);
    body_.push_back(round((monthly_pay - rate_.back()) * 100) / 100);
    date_pay.AddMonth();
    date_.push_back(date_pay.DateToStr());
    remainder_.push_back(remainder_.back() - body_.back());
  }
  if (count_payment_ > 1) {
    body_.back() = remainder_[remainder_.size() - 2];
    payment_.back() = rate_.back() + body_.back();
    remainder_.back() = remainder_[remainder_.size() - 2] - body_.back();
  }
}

extern "C" s21::ModelCalc *create_calculator() { return new s21::ModelCalc(); }

extern "C" void destroy_calculator(s21::ModelCalc *calculator) {
  delete calculator;
}

extern "C" bool parsing(s21::ModelCalc *calculator, const char *expression,
                        const double x) {
  return calculator->Parsing(expression, x);
}

extern "C" double calculate(s21::ModelCalc *calculator, const double x) {
  return calculator->Calculate(x);
}

extern "C" const char *result(s21::ModelCalc *calculator) {
  static std::string result;
  result = calculator->rezult();
  return result.c_str();
}

// �������������� ������� ��� CreditCalc
extern "C" s21::CreditCalc* create_credit_calculator() {
    return new s21::CreditCalc();
}

extern "C" void destroy_credit_calculator(s21::CreditCalc* calculator) {
    delete calculator;
}

extern "C" void credit_calc(s21::CreditCalc* calculator, double amount, double interest, int type, int months, int years, const char* sdate) {
    calculator->credit_calc(amount, interest, type, months, years, sdate);
}

extern "C" const char* credit_date(s21::CreditCalc* calculator, int index) {
    return calculator->date(index).c_str();
}

extern "C" double credit_payment(s21::CreditCalc* calculator, int index) {
    return calculator->payment(index);
}

extern "C" double credit_rate(s21::CreditCalc* calculator, int index) {
    return calculator->rate(index);
}

extern "C" double credit_body(s21::CreditCalc* calculator, int index) {
    return calculator->body(index);
}

extern "C" double credit_remainder(s21::CreditCalc* calculator, int index) {
    return calculator->remainder(index);
}

extern "C" double credit_overpayment(s21::CreditCalc* calculator) {
    return calculator->overpayment();
}

extern "C" double credit_total_payment(s21::CreditCalc* calculator) {
    return calculator->total_payment();
}

extern "C" int credit_count_payment(s21::CreditCalc* calculator) {
    return calculator->count_payment();
}
