# uwa
Universal Web Api

Sample file is attached


<p>Endpoint: <strong class="text-danger">/api/v1/ApplyCommit</strong></p>

<strong>select query with parameter</strong>
<pre>
<code>
{
  "SF_NAME": "sqldb1-sql1",
  "SF_RETN": "GET",
  "PARAM_LIST": [
                    {
                      "TEXT": "@@PROJ_ID",
                      "VALUE": "1"
                    }
                ]
}
</code>
</pre>


<strong>select query without parameter</strong>
<pre>
<code>
{
  "SF_NAME": "sqldb1-sql3",
  "SF_RETN": "GET",
  "PARAM_LIST": []
}
</code>
</pre>


<strong>insert query with parameter</strong>
<pre>
<code>
{
  "SF_NAME": "sqldb1-sql2",
  "SF_RETN": "POST",
  "PARAM_LIST": [
                   {
                      "TEXT": "@@ID",
                      "VALUE": "1"
                   },
                   {
                      "TEXT": "@@ROLE_NAME",
                      "VALUE": "Admin User"
                   }
                ]
}
</code>
</pre>

