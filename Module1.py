# code from Module1.vb

def ReadFile(FilePath):
	with open(FilePath,'r') as f:
		return f.read()

def SaveFile(FilePath, Data):
	with open(FilePath,'w') as f:
		f.write(Data)

def ReadPluginRecord(DBLocation, SQLStatement, ColumnArray):
	# TODO: line 26 Module1.vb
	pass

def PluginExecute(SQLQuery):
	# TODO: line 76 Module1.vb
	pass

def DbReadRecord(DBLocation, SQLStatement, ColumnArray):
	# TODO: line 108 Module1.vb
	pass

def DbExecute(SQLQuery):
	# TODO: line 206 Module1.vb
	pass

def TestMYSQL(connectionstring):
	# TODO: line 263 Module1.vb
	pass

def TestMYSQLite(connectionstring):
	# TODO: line 284 Module1.vb
	pass

def testMysql2(DBLocation, SQLStatement, ColumnArray):
	# TODO line 305 Module1.vb
	pass