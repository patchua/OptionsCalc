﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OptionsCalc
{
    class Entities
    {
        
        [SerializableAttribute]
        public enum OptionType { Call = 0, Put };

        [SerializableAttribute]
        public enum InstrumentType { Futures=0, Index, Bond, Option, Stock}

        [Serializable]
        public class Instrument : INotifyPropertyChanged
        {
            public Instrument(int id, string sec_code, string cl, InstrumentType type, string full_name, OptionType? o_type, double? strike, int? basec)
            {
                this._Class = cl;
                this._Code = sec_code;
                this._FullName = full_name;
                this._Id = id;
                this._Type = type;
                this._OptionType = o_type ;
                this._Strike = strike; 
                this._BaseContract = basec; 
            }

            public Instrument(int id, string sec_code, string cl, InstrumentType type, string full_name):this(id,sec_code,cl,type,full_name,null,null,null)
            { 
                
            }

            private string _Code;
            private string _Class;
            private string _FullName;
            private InstrumentType _Type;
            private int _Id;
            private OptionType? _OptionType;
            private double? _Strike;
            private int? _BaseContract;
            private double _LastPrice;
            private double _Volatility;
            private int _DaysToMate;
            private DateTime _MaturityDate;
            private double? _TheorPrice;

            public string Code { get{return _Code;} }

            public string FullName { get { return _FullName; } }

            public string Class { get { return _Class; } }

            public int Id { get { return _Id; } }

            public InstrumentType Type { get { return _Type; } }

            public OptionType? OptionType { get { return _OptionType; } }

            public double LastPrice { get{ return this._LastPrice;} set
            {
                if (value != this._LastPrice)
                {
                    this._LastPrice = value;
                    NotifyPropertyChanged("LastPrice");
                }
            }}

            public double? Strike { get { return _Strike; } }

            public double Volatility
            {
                get { return this._Volatility; }
                set
            {
                if (value != this._Volatility)
                {
                    this._Volatility = value;
                    NotifyPropertyChanged("Volatility");
                }
            } }

            public int DaysToMate { get { return this._DaysToMate; } set 
            {
                if (value != this._DaysToMate)
                {
                    this._DaysToMate = value;
                    NotifyPropertyChanged("DaysToMate");
                }
            } }

            public double? TheorPrice { get { return this._TheorPrice; } set 
            {
                if (value != this._TheorPrice)
                {
                    this._TheorPrice = value;
                    NotifyPropertyChanged("TheorPrice");
                }
            } }

            public DateTime MaturityDate { get { return this._MaturityDate; } set 
            {
                if (this._MaturityDate != value)
                {
                    this._MaturityDate = value;
                    NotifyPropertyChanged("MaturityDate");
                }
            } }

            public double Delta 
            {
                get 
                {
                    if (this._Type == InstrumentType.Futures)
                    {
                        return 1;
                    }
                    else if (this._Type == InstrumentType.Option && this._OptionType!=null)
                    {
                        return Quant.CalculateDelta((Entities.OptionType)this._OptionType, this._LastPrice, (double)this._Strike, this._Volatility, this._DaysToMate, Quant.RiskFreeRate);
                    }
                    else return 0;
                }
            }

            public double Gamma
            {
                get
                {
                    if (this._Type == InstrumentType.Option && this._OptionType!=null)
                    {
                        return Quant.CalculateGamma(this._LastPrice, (double)this._Strike, this._Volatility, this._DaysToMate, Quant.RiskFreeRate);
                    }
                    else return 0;
                }
            }

            public double Vega
            {
                get
                {
                    if (this._Type == InstrumentType.Option && this._OptionType != null)
                    {
                        return Quant.CalculateVega((Entities.OptionType)this._OptionType, this._LastPrice, (double)this._Strike, this._Volatility, this._DaysToMate, Quant.RiskFreeRate);
                    }
                    else return 0;
                }
            }

            public double Thetha
            {
                get
                {
                    if (this._Type == InstrumentType.Option && this._OptionType != null)
                    {
                        return Quant.CalculateThetha((Entities.OptionType)this._OptionType, this._LastPrice, (double)this._Strike, this._Volatility, this._DaysToMate, Quant.RiskFreeRate);
                    }
                    else return 0;
                }
            }

            public double Rho
            {
                get
                {
                    if (this._Type == InstrumentType.Option && this._OptionType != null)
                    {
                        return Quant.CalculateRho((Entities.OptionType)this._OptionType, this._LastPrice, (double)this._Strike, this._Volatility, this._DaysToMate, Quant.RiskFreeRate);
                    }
                    else return 0;
                }
            }

            public override string ToString()
            {
                return base.ToString();
            }

            public int? BaseContract { get { return _BaseContract; } }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string propertyName )
            {
                PropertyChangedEventHandler t = PropertyChanged;
                if (t != null)
                {
                    t(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        [Serializable]
        public class Account 
        {
            public Account(string name, int id)
            {
                this._Name = name;
                this._Id = id;
            }
            private string _Name;
            private int _Id;

            public string Name { get { return _Name; } }

            public int Id { get { return _Id; } }

            public override string ToString()
            {
                return base.ToString();
            }
        }

        [Serializable]
        public class Position : INotifyPropertyChanged
        {
            public Position(Instrument instr):this(instr,0,0,0)
            {
             
            }
            public Position(Instrument instr, int tnet, int bq, int sq)
            {
                this._Instrument = instr;
                this.BuyQty = bq;
                this.SellQty = sq;
                this.TotalNet = tnet;
            }
            private Instrument _Instrument;
            private int _TotalNet;
            private int _BuyQty;
            private int _SellQty;

            public int TotalNet
            {
                get { return this._TotalNet; }
                set
                {
                    if (value != this._TotalNet)
                    {
                        this._TotalNet = value;
                        NotifyPropertyChanged();
                    }
                } 
            }

            public int BuyQty { get { return this._BuyQty; } set 
            {
                if (value != this._BuyQty)
                {
                    this._BuyQty = value;
                    NotifyPropertyChanged();
                }
            } }

            public int SellQty { get { return this._SellQty; } set 
            {
                if (value != this._SellQty)
                {
                    this._SellQty = value;
                    NotifyPropertyChanged();
                }
            } }

            public Instrument Instrument { get { return _Instrument; } }

            public override string ToString()
            {
                return base.ToString();
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string propertyName="")
            {
                PropertyChangedEventHandler t = PropertyChanged;
                if (t != null)
                {
                    t(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        [Serializable]
        public class Portfolio : INotifyPropertyChanged
        {
            public Portfolio(int instrId, Account acc)
            {
                this._BaseId = instrId;
                this._Positions=new BindingList<Position>();
                this._Positions.ListChanged += _OnListChange;
                this._Account = acc;

            }
      
            public Portfolio(int instrId, Account acc, BindingList<Position> pos)
            {
                this._BaseId = instrId;

                this._Account = acc;
                this._Positions = pos;
                
                _Positions.ListChanged += _OnListChange;
            }
            void _OnListChange(object sender, ListChangedEventArgs e)
            {
                this.Refresh();
            }
            
            private double _Delta;
            private double _Gamma;
            private double _Vega;
            private double _Thetha;
            private double _Rho;
            private int _BaseId;
            private Account _Account;
            private BindingList<Position> _Positions;

            public string Name { get; set; }

            public int BaseId { get { return _BaseId; } }

            public Account Account { get { return _Account; } }

            public BindingList<Position> Positions
            {
                get { return _Positions; }
                set
                {
                    this._Positions = value;
                }
            }

            public  double Delta { get { return _Delta; } }

            public double Gamma { get { return _Gamma; } }

            public double Vega { get { return _Vega; } }
            
            public double Thetha { get { return _Thetha; } }
            
            public double Rho { get { return _Rho; } }

            public void Refresh ()
            {
                this._Delta = 0;
                this._Gamma = 0;
                this._Rho = 0;
                this._Thetha = 0;
                this._Vega = 0;
                foreach (Position pos in Positions)
                {
                    this._Delta += pos.TotalNet * pos.Instrument.Delta;
                    if (pos.Instrument.OptionType != null)
                    {
                        this._Gamma += pos.TotalNet * pos.Instrument.Gamma;
                        this._Vega += pos.TotalNet * pos.Instrument.Vega;
                        this._Thetha += pos.TotalNet * pos.Instrument.Thetha;
                        this._Rho += pos.TotalNet * pos.Instrument.Rho;
                    }
                }
                NotifyPropertyChanged();
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

        }
    }
}
