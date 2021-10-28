Position 2:

9/29/2021 9:25:07 PM -  -  - message -<prov.DbConnectionHelper.ConnectionString_Set|API> 1, 'Data Source=;Initial Catalog=;Integrated Security=True;Connect Timeout=6;Encrypt=False;IP Address Preference=IPv4First'
9/29/2021 9:25:07 PM -  -  - message -<prov.DbConnectionHelper.ConnectionString_Get|API> 1
9/29/2021 9:25:07 PM -  -  - message -SqlCommand.Set_Connection | API | ObjectId 1, Client Connection Id 00000000-0000-0000-0000-000000000000
9/29/2021 9:25:07 PM -  -  - message -SqlCommand.Set_CommandText | API | Object Id 1, String Value = '', Client Connection Id 00000000-0000-0000-0000-000000000000
9/29/2021 9:25:07 PM -  -  - message -<sc.AppConfigManager.FetchConfigurationSection|INFO>: Unable to load custom `AppContextSwitchOverrides`. Default value of `T` type returns.
9/29/2021 9:25:07 PM -  -  - message -<sc.SqlAppContextSwitchManager.ApplyContextSwitches|INFO> Entry point.
9/29/2021 9:25:07 PM -  -  - message -<sc.SqlAppContextSwitchManager.ApplyContextSwitches|INFO> Exit point.
9/29/2021 9:25:07 PM - ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use. -  - message -ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use.
9/29/2021 9:25:07 PM -  -  - message -<sc|SqlAuthenticationProviderManager|Ctor|Info>Neither SqlClientAuthenticationProviders nor SqlAuthenticationProviders configuration section found.
9/29/2021 9:25:07 PM -  -  - message -TdsParserStateObjectFactory.CreateTdsParserStateObject | Info | AppContext switch 'Switch.Microsoft.Data.SqlClient.UseManagedNetworkingOnWindows' not enabled, native networking implementation will be used.
9/29/2021 9:25:07 PM -  -  - message -TdsParser.Connect | SEC | Connection Object Id 4, Authentication Mode: SqlPassword
9/29/2021 9:25:07 PM -  -  - message -TdsParser.Connect | SEC | SSPI or Active Directory Authentication Library loaded for SQL Server based integrated authentication
9/29/2021 9:25:07 PM -  -  - message -<sc.TdsParser.Connect|SEC> Sending prelogin handshake
9/29/2021 9:25:07 PM -  -  - message -<sc.TdsParser.SendPreLoginHandshake|INFO> ClientConnectionID 6ff0e013-3b6f-4bb2-853a-aee70ffe8cae, ActivityID 73e1d6bd-62c3-4340-944f-de8b9200e2e5:2
9/29/2021 9:25:07 PM -  -  - message -<sc.TdsParser.Connect|SEC> Consuming prelogin handshake
Security Warning: The negotiated TLS 1.0 is an insecure protocol and is supported for backward compatibility only. The recommended protocol version is TLS 1.2 and later.
9/29/2021 9:25:07 PM -  -  - message -<sc|TdsParser|ConsumePreLoginHandshake|Warning>Security Warning: The negotiated TLS 1.0 is an insecure protocol and is supported for backward compatibility only. The recommended protocol version is TLS 1.2 and later.
9/29/2021 9:25:07 PM -  -  - message -<sc.TdsParser.TryRun|SEC> Received login acknowledgement token
9/29/2021 9:25:07 PM -  -  - message -<sc|ActiveDirectoryAuthenticationTimeoutRetryHelper|SetState|Info>State changed from NotStarted to HasLoggedIn.
9/29/2021 9:25:07 PM -  -  - message -<prov.DbConnectionFactory.CreatePooledConnection|RES|CPOOL> 1, Pooled database connection created.
9/29/2021 9:25:07 PM -  -  - message -SqlCommand.RunExecuteReaderTds | Info | Object Id 1, Activity Id 73e1d6bd-62c3-4340-944f-de8b9200e2e5:2, Client Connection Id 6ff0e013-3b6f-4bb2-853a-aee70ffe8cae, Command executed as SQLBATCH, Command Text ''
9/29/2021 9:25:09 PM -  -  - message -TdsParserStateObject.DecrementOpenResultCount | INFO | State Object Id 1, Processing Attention.


Position 0:

9/29/2021 9:32:59 PM -  -  - message -SqlConnection.Open | API | Correlation | Object Id 1, Activity Id 1a3851bb-69f9-48b8-8f45-c18ce844fddf:1
9/29/2021 9:32:59 PM - ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use. -  - message -ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use.
Security Warning: The negotiated TLS 1.0 is an insecure protocol and is supported for backward compatibility only. The recommended protocol version is TLS 1.2 and later.
9/29/2021 9:32:59 PM -  -  - message -SqlCommand.ExecuteDbDataReader | API | Correlation | Object Id 1, Activity Id 2d0f0bef-7c92-410d-9d6d-dbe0487b515d:2, Client Connection Id 138009fb-3def-412b-b573-8ca624715d79, Command Text ''
9/29/2021 9:33:01 PM -  -  - message -SqlConnection.Close | API | Correlation | Object Id 1, Activity Id 2d0f0bef-7c92-410d-9d6d-dbe0487b515d:2, Client Connection Id 138009fb-3def-412b-b573-8ca624715d79

Position 0 & 1:

9/29/2021 9:33:33 PM -  -  - message -SqlConnection.Open | API | Correlation | Object Id 1, Activity Id 84906486-b8b1-47f7-8412-670109942959:1
9/29/2021 9:33:33 PM - ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use. -  - message -ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use.
Security Warning: The negotiated TLS 1.0 is an insecure protocol and is supported for backward compatibility only. The recommended protocol version is TLS 1.2 and later.
9/29/2021 9:33:33 PM -  -  - message -SqlCommand.ExecuteDbDataReader | API | Correlation | Object Id 1, Activity Id 54b13b4b-d3ec-4b6f-9d9a-77aec47e83a8:2, Client Connection Id d7077125-bf8c-4c98-b754-49bdab4a47f0, Command Text ''
9/29/2021 9:33:33 PM -  -  - message -SqlCommand.WriteBeginExecuteEvent | INFO | Object Id 1, Client Connection Id d7077125-bf8c-4c98-b754-49bdab4a47f0, Command Text 
9/29/2021 9:33:33 PM -  -  - message -SqlCommand.WriteEndExecuteEvent | INFO | Object Id 1, Client Connection Id d7077125-bf8c-4c98-b754-49bdab4a47f0, Composite State 4, Sql Exception Number 0
9/29/2021 9:33:35 PM -  -  - message -SqlConnection.Close | API | Correlation | Object Id 1, Activity Id 54b13b4b-d3ec-4b6f-9d9a-77aec47e83a8:2, Client Connection Id d7077125-bf8c-4c98-b754-49bdab4a47f0

Position 1:

9/29/2021 9:34:31 PM - ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use. -  - message -ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use.
Security Warning: The negotiated TLS 1.0 is an insecure protocol and is supported for backward compatibility only. The recommended protocol version is TLS 1.2 and later.
9/29/2021 9:34:31 PM -  -  - message -SqlCommand.WriteBeginExecuteEvent | INFO | Object Id 1, Client Connection Id d36100b2-c0af-4ee8-872c-bd46842ea39e, Command Text 
9/29/2021 9:34:31 PM -  -  - message -SqlCommand.WriteEndExecuteEvent | INFO | Object Id 1, Client Connection Id d36100b2-c0af-4ee8-872c-bd46842ea39e, Composite State 4, Sql Exception Number 0

Position 0 & 1 with 2 queries:


9/29/2021 9:40:43 PM -  -  - message -SqlConnection.Open | API | Correlation | Object Id 1, Activity Id dfc1a1b1-e8d3-41d7-9661-507d8f1d566f:1
9/29/2021 9:40:43 PM - ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use. -  - message -ERROR: Exception in Command Processing for EventSource System.Transactions.TransactionsEventSource: Event SetActivityId has ID 40 which is already in use.
Security Warning: The negotiated TLS 1.0 is an insecure protocol and is supported for backward compatibility only. The recommended protocol version is TLS 1.2 and later.
9/29/2021 9:40:43 PM -  -  - message -SqlCommand.ExecuteDbDataReader | API | Correlation | Object Id 1, Activity Id f8d62be4-d34f-41ae-bf63-313879b5ff3e:2, Client Connection Id 6b832731-eca7-449d-8552-dd174b00074e, Command Text ''
9/29/2021 9:40:43 PM -  -  - message -SqlCommand.WriteBeginExecuteEvent | INFO | Object Id 1, Client Connection Id 6b832731-eca7-449d-8552-dd174b00074e, Command Text 
9/29/2021 9:40:43 PM -  -  - message -SqlCommand.WriteEndExecuteEvent | INFO | Object Id 1, Client Connection Id 6b832731-eca7-449d-8552-dd174b00074e, Composite State 4, Sql Exception Number 0
9/29/2021 9:40:43 PM -  -  - message -SqlConnection.Close| API | Correlation | Object Id 1, Activity Id f8d62be4-d34f-41ae-bf63-313879b5ff3e:2, Client Connection Id 6b832731-eca7-449d-8552-dd174b00074e

9/29/2021 9:40:43 PM -  -  - message -SqlConnection.Open | API | Correlation | Object Id 2, Activity Id f8d62be4-d34f-41ae-bf63-313879b5ff3e:2
9/29/2021 9:40:43 PM -  -  - message -SqlCommand.ExecuteDbDataReader | API | Correlation | Object Id 2, Activity Id f8d62be4-d34f-41ae-bf63-313879b5ff3e:2, Client Connection Id 6b832731-eca7-449d-8552-dd174b00074e, Command Text ''
9/29/2021 9:40:43 PM -  -  - message -SqlCommand.WriteBeginExecuteEvent | INFO | Object Id 2, Client Connection Id 6b832731-eca7-449d-8552-dd174b00074e, Command Text 
9/29/2021 9:40:43 PM -  -  - message -SqlCommand.WriteEndExecuteEvent | INFO | Object Id 2, Client Connection Id 6b832731-eca7-449d-8552-dd174b00074e, Composite State 4, Sql Exception Number 0
9/29/2021 9:40:43 PM -  -  - message -SqlConnection.Close | API | Correlation | Object Id 2, Activity Id f8d62be4-d34f-41ae-bf63-313879b5ff3e:2, Client Connection Id 6b832731-eca7-449d-8552-dd174b00074e

# Closely examining various position toggles such as 0,6, and 2:

Position 2:
Has decrement open result count that happens right after result set is returned apparently, but does not have any ID's for correlation aside from ObjectId. No Activity ID and no ConnectionId.
Might be useful for somethings but seems not super useful for tracking time for queries.

From position 0:
First we have a ExecuteDbDataReader with an (ObjectId, ActivityId, & ConnectionId)
We can see the command text is included with that event.

From position 1:
There is also a WriteBeginExecuteEvent which also has the (ObjectId, _, ConnectionId) and the same command text.
There is also a WriteEndExecuteEvent which has the same (ObjectId, _, ConnectionId) and whatever a composite state and sql exception number are. This occurs before the data is ready from sql server so cannot be used for timing.

From position 0:
We see a SqlConnectionClose event which shares the same (ObjectId, ActivityId, & ConnectionId) as the ExecuteDbDataReader and occurs after the data is read. This looks like it can be used for timing.

So, in a nutshell, looks like position 0 gives us all we need.




