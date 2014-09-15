﻿using System.Collections;

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
namespace org.apache.lucene.queries.function.valuesource
{

	using AtomicReaderContext = org.apache.lucene.index.AtomicReaderContext;
	using IndexReader = org.apache.lucene.index.IndexReader;
	using ReaderUtil = org.apache.lucene.index.ReaderUtil;


	/// <summary>
	/// Returns the value of <seealso cref="IndexReader#numDocs()"/>
	/// for every document. This is the number of documents
	/// excluding deletions.
	/// </summary>
	public class NumDocsValueSource : ValueSource
	{
	  public virtual string name()
	  {
		return "numdocs";
	  }

	  public override string description()
	  {
		return name() + "()";
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public org.apache.lucene.queries.function.FunctionValues getValues(java.util.Map context, org.apache.lucene.index.AtomicReaderContext readerContext) throws java.io.IOException
	  public override FunctionValues getValues(IDictionary context, AtomicReaderContext readerContext)
	  {
		// Searcher has no numdocs so we must use the reader instead
		return new ConstIntDocValues(ReaderUtil.getTopLevelContext(readerContext).reader().numDocs(), this);
	  }

	  public override bool Equals(object o)
	  {
		return this.GetType() == o.GetType();
	  }

	  public override int GetHashCode()
	  {
		return this.GetType().GetHashCode();
	  }
	}

}