﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using org.apache.lucene.queries;

namespace Lucene.Net.Queries
{

	/*
	 * Licensed to the Apache Software Foundation (ASF) under one or more
	 * contributor license agreements.  See the NOTICE file distributed with
	 * this work for additional information regarding copyright ownership.
	 * The ASF licenses this file to You under the Apache License, Version 2.0
	 * (the "License"); you may not use this file except in compliance with
	 * the License.  You may obtain a copy of the License at
	 *
	 *     http://www.apache.org/licenses/LICENSE-2.0
	 *
	 * Unless required by applicable law or agreed to in writing, software
	 * distributed under the License is distributed on an "AS IS" BASIS,
	 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	 * See the License for the specific language governing permissions and
	 * limitations under the License.
	 */
    /// <summary>
	/// A container Filter that allows Boolean composition of Filters.
	/// Filters are allocated into one of three logical constructs;
	/// SHOULD, MUST NOT, MUST
	/// The results Filter BitSet is constructed as follows:
	/// SHOULD Filters are OR'd together
	/// The resulting Filter is NOT'd with the NOT Filters
	/// The resulting Filter is AND'd with the MUST Filters
	/// </summary>
	public class BooleanFilter : Filter, IEnumerable<FilterClause>
	{

	  private readonly IList<FilterClause> clauses_Renamed = new List<FilterClause>();

	  /// <summary>
	  /// Returns the a DocIdSetIterator representing the Boolean composition
	  /// of the filters that have been added.
	  /// </summary>
	  public override DocIdSet GetDocIdSet(AtomicReaderContext context, Bits acceptDocs)
	  {
		FixedBitSet res = null;
		AtomicReader reader = context.reader();

		bool hasShouldClauses = false;
		foreach (FilterClause fc in clauses_Renamed)
		{
		  if (fc.Occur == BooleanClause.Occur.SHOULD)
		  {
			hasShouldClauses = true;
			DocIdSetIterator disi = getDISI(fc.Filter, context);
			if (disi == null)
			{
				continue;
			}
			if (res == null)
			{
			  res = new FixedBitSet(reader.MaxDoc());
			}
			res.or(disi);
		  }
		}
		if (hasShouldClauses && res == null)
		{
		  return null;
		}

		foreach (FilterClause fc in clauses_Renamed)
		{
		  if (fc.Occur == BooleanClause.Occur.MUST_NOT)
		  {
			if (res == null)
			{
			  Debug.Assert(!hasShouldClauses);
			  res = new FixedBitSet(reader.MaxDoc());
			  res.Set(0, reader.MaxDoc()); // NOTE: may set bits on deleted docs
			}

              DocIdSetIterator disi = GetDISI(fc.Filter, context);
			if (disi != null)
			{
			  res.AndNot(disi);
			}
		  }
		}

		foreach (FilterClause fc in clauses_Renamed)
		{
		  if (fc.Occur == BooleanClause.Occur.MUST)
		  {
			DocIdSetIterator disi = GetDISI(fc.Filter, context);
			if (disi == null)
			{
			  return null; // no documents can match
			}
			if (res == null)
			{
			  res = new FixedBitSet(reader.maxDoc());
			  res.or(disi);
			}
			else
			{
			  res.and(disi);
			}
		  }
		}

		return BitsFilteredDocIdSet.wrap(res, acceptDocs);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private static org.apache.lucene.search.DocIdSetIterator getDISI(org.apache.lucene.search.Filter filter, org.apache.lucene.index.AtomicReaderContext context) throws java.io.IOException
	  private static DocIdSetIterator GetDISI(Filter filter, AtomicReaderContext context)
	  {
		// we dont pass acceptDocs, we will filter at the end using an additional filter
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.apache.lucene.search.DocIdSet set = filter.getDocIdSet(context, null);
		DocIdSet set = filter.GetDocIdSet(context, null);
		return set == null ? null : set.GetEnumerator();
	  }

	  /// <summary>
	  /// Adds a new FilterClause to the Boolean Filter container </summary>
	  /// <param name="filterClause"> A FilterClause object containing a Filter and an Occur parameter </param>
	  public virtual void Add(FilterClause filterClause)
	  {
		clauses_Renamed.Add(filterClause);
	  }

	  public void Add(Filter filter, BooleanClause.Occur occur)
	  {
		Add(new FilterClause(filter, occur));
	  }

	  /// <summary>
	  /// Returns the list of clauses
	  /// </summary>
	  public virtual IList<FilterClause> clauses()
	  {
		return clauses_Renamed;
	  }

	  /// <summary>
	  /// Returns an iterator on the clauses in this query. It implements the <seealso cref="Iterable"/> interface to
	  /// make it possible to do:
	  /// <pre class="prettyprint">for (FilterClause clause : booleanFilter) {}</pre>
	  /// </summary>
	  public IEnumerator<FilterClause> GetEnumerator()
	  {
		return clauses().GetEnumerator();
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}

		if ((obj == null) || (obj.GetType() != this.GetType()))
		{
		  return false;
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final BooleanFilter other = (BooleanFilter)obj;
		BooleanFilter other = (BooleanFilter)obj;
		return clauses_Renamed.Equals(other.clauses_Renamed);
	  }

	  public override int GetHashCode()
	  {
		return 657153718 ^ clauses_Renamed.GetHashCode();
	  }

	  /// <summary>
	  /// Prints a user-readable version of this Filter. </summary>
	  public override string ToString()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StringBuilder buffer = new StringBuilder("BooleanFilter(");
		StringBuilder buffer = new StringBuilder("BooleanFilter(");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int minLen = buffer.length();
		int minLen = buffer.Length;
		foreach (FilterClause c in clauses_Renamed)
		{
		  if (buffer.Length > minLen)
		  {
			buffer.Append(' ');
		  }
		  buffer.Append(c);
		}
		return buffer.Append(')').ToString();
	  }
	}

}