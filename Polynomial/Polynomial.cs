using System;
using System.Collections;
using System.Collections.Generic;
using PolynomialObject.Exceptions;

namespace PolynomialObject
{
    public sealed class Polynomial : IEnumerable<PolynomialMember>
    {
        List<PolynomialMember> PolynomialMembers;
        public Polynomial()
        {
            PolynomialMembers = new List<PolynomialMember>();
        }

        public Polynomial(PolynomialMember member)
        {
            PolynomialMembers = new List<PolynomialMember>();
            PolynomialMembers.Add(member);
        }

        public Polynomial(IEnumerable<PolynomialMember> members)
        {
            PolynomialMembers = new List<PolynomialMember>();
            foreach (var item in members)
            {
                PolynomialMembers.Add(item);
            }
            //PolynomialMembers.Sort((a, b) => a.Degree.CompareTo(b.Degree));
        }

        public Polynomial((double degree, double coefficient) member)
        {
            PolynomialMembers = new List<PolynomialMember>();
            this.AddMember(new PolynomialMember(member.degree, member.coefficient));
        }

        public Polynomial(IEnumerable<(double degree, double coefficient)> members)
        {
            PolynomialMembers = new List<PolynomialMember>();
            foreach (var item in members)
            {
                PolynomialMembers.Add(new PolynomialMember(item.degree, item.coefficient));
            }
        }

        /// <summary>
        /// The amount of not null polynomial members in polynomial 
        /// </summary>
        public int Count
        {
            get
            {
                int x = 0;
                foreach(PolynomialMember item in PolynomialMembers)
                {
                    if (!(item is null)) x++;
                }
                return x;
            }
        }

        /// <summary>
        /// The biggest degree of polynomial member in polynomial
        /// </summary>
        public double Degree
        {
            get
            {
                double max = 0;
                foreach (PolynomialMember item in PolynomialMembers)
                {
                    if (!(item is null) && item.Degree > max) max = item.Degree;
                }
                return max;
            }
        }

        /// <summary>
        /// Adds new unique member to polynomial 
        /// </summary>
        /// <param name="member">The member to be added</param>
        /// <exception cref="PolynomialArgumentException">Throws when member to add with such degree already exist in polynomial</exception>
        /// <exception cref="PolynomialArgumentNullException">Throws when trying to member to add is null</exception>
        public void AddMember(PolynomialMember member)
        {
            //if (member.Coefficient == 0) throw new PolynomialArgumentException();
            if (member is null) throw new PolynomialArgumentNullException();
            else if (PolynomialMembers.Exists(x => x.Degree == member.Degree)) throw new PolynomialArgumentException();
            else PolynomialMembers.Add(member);
        }

        /// <summary>
        /// Adds new unique member to polynomial from tuple
        /// </summary>
        /// <param name="member">The member to be added</param>
        /// <exception cref="PolynomialArgumentException">Throws when member to add with such degree already exist in polynomial</exception>
        public void AddMember((double degree, double coefficient) member)
        {
            if (member.coefficient == 0) throw new PolynomialArgumentException();
            else if (PolynomialMembers.Exists(x => x.Degree == member.degree)) throw new PolynomialArgumentException();
            else PolynomialMembers.Add(new PolynomialMember(member.degree, member.coefficient));
        }

        /// <summary>
        /// Removes member of specified degree
        /// </summary>
        /// <param name="degree">The degree of member to be deleted</param>
        /// <returns>True if member has been deleted</returns>
        public bool RemoveMember(double degree)
        {
            if (this.ContainsMember(degree))
            {
                PolynomialMembers.Remove(PolynomialMembers.Find(x => x.Degree == degree));
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Searches the polynomial for a method of specified degree
        /// </summary>
        /// <param name="degree">Degree of member</param>
        /// <returns>True if polynomial contains member</returns>
        public bool ContainsMember(double degree)
        {
            return PolynomialMembers.Exists(x => x.Degree == degree);
        }

        /// <summary>
        /// Finds member of specified degree
        /// </summary>
        /// <param name="degree">Degree of member</param>
        /// <returns>Returns the found member or null</returns>
        public PolynomialMember Find(double degree)
        {
            return PolynomialMembers.Find(x => x.Degree == degree) as PolynomialMember;
        }

        /// <summary>
        /// Gets and sets the coefficient of member with provided degree
        /// If there is no null member for searched degree - return 0 for get and add new member for set
        /// </summary>
        /// <param name="degree">The degree of searched member</param>
        /// <returns>Coefficient of found member</returns>
        public double this[double degree]
        {
            get
            {
                if (PolynomialMembers.Find(x => x.Degree == degree) != null) return PolynomialMembers.Find(x => x.Degree == degree).Coefficient;
                else return 0;
            }
            set
            {
                if (PolynomialMembers.Exists(x => x.Degree == degree))
                {
                    if (value == 0)
                    {
                        this.RemoveMember(PolynomialMembers.Find(x => x.Degree == degree).Degree);
                    }
                    else PolynomialMembers.Find(x => x.Degree == degree).Coefficient = value;
                }    
                else
                {
                    if(value != 0) this.AddMember(new PolynomialMember(degree, value));
                }

                    
            }
        }

        /// <summary>
        /// Convert polynomial to array of included polynomial members 
        /// </summary>
        /// <returns>Array with not null polynomial members</returns>
        public PolynomialMember[] ToArray()
        {
            PolynomialMember[] arr = new PolynomialMember[PolynomialMembers.Count];
            int i = 0;
            foreach(var item in PolynomialMembers)
            {
                if (item != null) arr[i] = item;
                i++;
            }
            return arr;
        }

        /// <summary>
        /// Adds two polynomials
        /// </summary>
        /// <param name="a">The first polynomial</param>
        /// <param name="b">The second polynomial</param>
        /// <returns>New polynomial after adding</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if either of provided polynomials is null</exception>
        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            if (a is null) throw new PolynomialArgumentNullException();
            else if (b is null) throw new PolynomialArgumentNullException();
            else
            {
                    for (int i = 0; i < b.Count; i++)
                    {
                        if (a.PolynomialMembers.Exists(x => x.Degree == b.ToArray()[i].Degree))
                        {
                            var AElem = a.PolynomialMembers.Find(x => x.Degree == b.ToArray()[i].Degree);
                            var BElem = b.ToArray()[i];
                            if (BElem.Coefficient + AElem.Coefficient != 0)
                                {
                                    a.RemoveMember(BElem.Degree);
                                    a.AddMember(new PolynomialMember(BElem.Degree, BElem.Coefficient + AElem.Coefficient));
                                }
                            else
                            {
                                a.RemoveMember(b.ToArray()[i].Degree);
                            }
                    }
                        else
                        {
                            if(b.ToArray()[i].Coefficient != 0) a.AddMember(b.ToArray()[i]);
                        }
                    }
                a.PolynomialMembers.RemoveAll(x => x.Coefficient == 0);
                return a;
            }
        }

        /// <summary>
        /// Subtracts two polynomials
        /// </summary>
        /// <param name="a">The first polynomial</param>
        /// <param name="b">The second polynomial</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if either of provided polynomials is null</exception>
        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            if (a is null) throw new PolynomialArgumentNullException();
            else if (b is null) throw new PolynomialArgumentNullException();
            else
            {
                foreach (var item in b)
                {
                    item.Coefficient *= -1;
                }
                foreach (var item in b)
                {
                    if (a.PolynomialMembers.Exists(x => x.Degree == item.Degree))
                    {
                        var AElem = a.PolynomialMembers.Find(x => x.Degree == item.Degree);
                        var BElem = item;
                        if (BElem.Coefficient + AElem.Coefficient != 0)
                        {
                            a.RemoveMember(BElem.Degree);
                            a.AddMember(new PolynomialMember(BElem.Degree, BElem.Coefficient + AElem.Coefficient));
                        }
                        else
                        {
                            a.RemoveMember(item.Degree);
                        }
                    }
                    else
                    {
                        if (item.Coefficient != 0) a.AddMember(item);
                    }
                }
                a.PolynomialMembers.RemoveAll(x => x.Coefficient == 0);
                return a;
            }
        }

        /// <summary>
        /// Multiplies two polynomials
        /// </summary>
        /// <param name="a">The first polynomial</param>
        /// <param name="b">The second polynomial</param>
        /// <returns>Returns new polynomial after multiplication</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if either of provided polynomials is null</exception>
        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            if (a is null) throw new PolynomialArgumentNullException();
            else if (b is null) throw new PolynomialArgumentNullException();
            else
            {
                Polynomial c = new Polynomial();
                foreach (var aitem in a)
                {
                    foreach (var bitem in b)
                    {
                        try
                        {
                            c.AddMember(new PolynomialMember(aitem.Degree + bitem.Degree, aitem.Coefficient * bitem.Coefficient));
                        }
                        catch (PolynomialArgumentException)
                        {
                            c.PolynomialMembers.Find(x => x.Degree == aitem.Degree + bitem.Degree).Coefficient += aitem.Coefficient * bitem.Coefficient;
                        }
                    }
                }
                c.PolynomialMembers.RemoveAll(x => x.Coefficient == 0);
                return c;
            }
        }

        /// <summary>
        /// Adds polynomial to polynomial
        /// </summary>
        /// <param name="polynomial">The polynomial to add</param>
        /// <returns>Returns new polynomial after adding</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if provided polynomial is null</exception>
        public Polynomial Add(Polynomial polynomial)
        {
            return this + polynomial;
        }

        /// <summary>
        /// Subtracts polynomial from polynomial
        /// </summary>
        /// <param name="polynomial">The polynomial to subtract</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if provided polynomial is null</exception>
        public Polynomial Subtraction(Polynomial polynomial)
        {
            return this - polynomial;
        }

        /// <summary>
        /// Multiplies polynomial with polynomial
        /// </summary>
        /// <param name="polynomial">The polynomial for multiplication </param>
        /// <returns>Returns new polynomial after multiplication</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if provided polynomial is null</exception>
        public Polynomial Multiply(Polynomial polynomial)
        {
            return this * polynomial;
        }

        /// <summary>
        /// Adds polynomial and tuple
        /// </summary>
        /// <param name="a">The polynomial</param>
        /// <param name="b">The tuple</param>
        /// <returns>Returns new polynomial after adding</returns>
        public static Polynomial operator +(Polynomial a, (double degree, double coefficient) b)
        {
            if (a.PolynomialMembers.Exists(x => x.Degree == b.degree))
            {
                var AElem = a.PolynomialMembers.Find(x => x.Degree == b.degree);
                if (b.coefficient + AElem.Coefficient != 0)
                {
                    a.RemoveMember(b.degree);
                    a.AddMember(new PolynomialMember(b.degree, b.coefficient + AElem.Coefficient));
                }
                else
                {
                    a.RemoveMember(b.degree);
                }
            }
            else
            {
                if (b.coefficient != 0) a.AddMember(b);
            }
            a.PolynomialMembers.RemoveAll(x => x.Coefficient == 0);
            return a;
        }

        /// <summary>
        /// Subtract polynomial and tuple
        /// </summary>
        /// <param name="a">The polynomial</param>
        /// <param name="b">The tuple</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        public static Polynomial operator -(Polynomial a, (double degree, double coefficient) b)
        {
            if (a.PolynomialMembers.Exists(x => x.Degree == b.degree))
            {
                var AElem = a.PolynomialMembers.Find(x => x.Degree == b.degree);
                if (AElem.Coefficient - b.coefficient != 0)
                {
                    a.RemoveMember(b.degree);
                    a.AddMember(new PolynomialMember(b.degree, AElem.Coefficient - b.coefficient));
                }
                else
                {
                    a.RemoveMember(b.degree);
                }
            }
            else
            {
                if (b.coefficient != 0)
                {
                    b.coefficient = -b.coefficient;
                    a.AddMember(b);
                } 
            }
            a.PolynomialMembers.RemoveAll(x => x.Coefficient == 0);
            return a;
        }

        /// <summary>
        /// Multiplies polynomial and tuple
        /// </summary>
        /// <param name="a">The polynomial</param>
        /// <param name="b">The tuple</param>
        /// <returns>Returns new polynomial after multiplication</returns>
        public static Polynomial operator *(Polynomial a, (double degree, double coefficient) b)
        {
            if (a is null) throw new PolynomialArgumentNullException();
            else
            {
                Polynomial c = new Polynomial();
                if (b.coefficient == 0) return c;
                foreach (var aitem in a)
                {
                    try
                    {
                        c.AddMember(new PolynomialMember(aitem.Degree + b.degree, aitem.Coefficient * b.coefficient));
                    }
                    catch (PolynomialArgumentException)
                    {
                        c.PolynomialMembers.Find(x => x.Degree == aitem.Degree + b.degree).Coefficient += aitem.Coefficient * b.coefficient;
                    }
                }
                c.PolynomialMembers.RemoveAll(x => x.Coefficient == 0);
                return c;
            }
        }

        /// <summary>
        /// Adds tuple to polynomial
        /// </summary>
        /// <param name="member">The tuple to add</param>
        /// <returns>Returns new polynomial after adding</returns>
        public Polynomial Add((double degree, double coefficient) member)
        {
            return this + member;
        }

        /// <summary>
        /// Subtracts tuple from polynomial
        /// </summary>
        /// <param name="member">The tuple to subtract</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        public Polynomial Subtraction((double degree, double coefficient) member)
        {
            return this - member;
        }

        /// <summary>
        /// Multiplies tuple with polynomial
        /// </summary>
        /// <param name="member">The tuple for multiplication </param>
        /// <returns>Returns new polynomial after multiplication</returns>
        public Polynomial Multiply((double degree, double coefficient) member)
        {
            return this * member;
        }

        public IEnumerator<PolynomialMember> GetEnumerator()
        {
            return PolynomialMembers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return PolynomialMembers.GetEnumerator();
        }
    }
}
