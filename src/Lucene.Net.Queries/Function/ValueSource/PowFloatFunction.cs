﻿using System;

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


	/// <summary>
	/// Function to raise the base "a" to the power "b"
	/// </summary>
	public class PowFloatFunction : DualFloatFunction
	{
	 /// <param name="a">  the base. </param>
	 /// <param name="b">  the exponent. </param>
	  public PowFloatFunction(ValueSource a, ValueSource b) : base(a,b)
	  {
	  }

	  protected internal override string name()
	  {
		return "pow";
	  }

	  protected internal override float func(int doc, FunctionValues aVals, FunctionValues bVals)
	  {
		return (float)Math.Pow(aVals.floatVal(doc), bVals.floatVal(doc));
	  }
	}



}