/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you
 * may not use this file except in compliance with the License. You may
 * obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * User: fyfej
 * Date: 1-9-2017
 */

using System.Collections.Generic;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration
{
	/// <summary>
	/// Stores configuration data related to a trigger event handler
	/// </summary>
	public class MessageHandlerConfiguration
	{
		/// <summary>
		/// Creates a new instance of the TriggerHandlerConfiguration
		/// </summary>
		public MessageHandlerConfiguration()
		{
			this.Interactions = new List<InteractionConfiguration>();
		}

		/// <summary>
		/// Gets or sets the handler that receives messages
		/// </summary>
		public IEverestMessageReceiver Handler { get; set; }

		/// <summary>
		/// Gets the list of interactions that the specified handler can process
		/// </summary>
		public List<InteractionConfiguration> Interactions { get; private set; }
	}
}