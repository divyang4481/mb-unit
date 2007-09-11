<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:g="http://www.mbunit.com/gallio">
  <xsl:output method="xml" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"
              doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" indent="yes" />

  <xsl:key name="outcome" match="/g:report/g:packageRun/g:testRuns/g:testRun" use="g:stepRun/g:result/@outcome" />
  <xsl:key name="state" match="/g:report/g:packageRun/g:testRuns/g:testRun" use="g:stepRun/g:result/@state" />

  <xsl:template match="g:report">
    <html>
      <head>
        <title>MbUnit Test Report</title>
        <link rel="stylesheet" type="text/css" href="css/MbUnit-Report.css" />
        <script language="javascript" type="text/javascript" src="js/MbUnit-Report.js">
          <xsl:comment> comment inserted for Internet Explorer </xsl:comment>
        </script>
      </head>
      <body>
        <img src="img/Logo.png" alt="MbUnit Logo" />
        <h1>MbUnit Test Report</h1>
        <xsl:apply-templates select="g:package/g:assemblyFiles" />
        <xsl:apply-templates select="g:packageRun" />
        <xsl:apply-templates select="g:testModel/g:test" />
        <xsl:apply-templates select="g:packageRun/g:testRuns" />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="g:package/g:assemblyFiles">
    <div id="Assemblies">
      <h2>Assemblies</h2>
      <ul>
        <xsl:for-each select="g:assemblyFile">
          <li>
            <xsl:value-of select="."/>
          </li>
        </xsl:for-each>
      </ul>
    </div>
  </xsl:template>

  <xsl:template match="g:packageRun">
    <div id="Statistics">
      <h2>Statistics</h2>
      <xsl:text>Start time: </xsl:text>
      <xsl:call-template name="format-datetime">
        <xsl:with-param name="datetime" select="@startTime" />
      </xsl:call-template>
      <br />
      <xsl:text> End time: </xsl:text>
      <xsl:call-template name="format-datetime">
        <xsl:with-param name="datetime" select="@endTime" />
      </xsl:call-template>
      <xsl:apply-templates select="g:statistics" />
    </div>
  </xsl:template>

  <xsl:template match="g:statistics">
    <table id="Stats" border="1">
      <tr>
        <th>Tests</th>
        <th>Run</th>
        <th>Passed</th>
        <th>Failed</th>
        <th>Inconclusive</th>
        <th>Ignored</th>
        <th>Skipped</th>
        <th>Asserts</th>
        <th>Duration</th>
      </tr>
      <tr>
        <td>
          <xsl:value-of select="@testCount" />
        </td>
        <td>
          <xsl:value-of select="@runCount" />
        </td>
        <td>
          <xsl:value-of select="@passCount" />
        </td>
        <td>
          <xsl:value-of select="@failureCount" />
        </td>
        <td>
          <xsl:value-of select="@inconclusiveCount" />
        </td>
        <td>
          <xsl:value-of select="@ignoreCount" />
        </td>
        <td>
          <xsl:value-of select="@skipCount" />
        </td>
        <td>
          <xsl:value-of select="@assertCount" />
        </td>
        <td>
          <xsl:value-of select="format-number(@duration, '0.00')" />s
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="g:testModel/g:test">
    <div id="Summary">
      <h2>Summary</h2>
      <ul>
        <xsl:apply-templates select="g:children/g:test" />
      </ul>
    </div>
  </xsl:template>

  <xsl:template match="g:children/g:test">
    <xsl:variable name="id" select="@id" />
    <xsl:variable name="tests" select="descendant-or-self::g:test[@isTestCase='true']" />
    <xsl:variable name="run" select="$tests[@id = key('state', 'executed')/@id]" />
    <xsl:variable name="passed" select="$tests[@id = key('outcome', 'passed')/@id]" />
    <xsl:variable name="failed" select="$tests[@id = key('outcome', 'failed')/@id]" />
    <xsl:variable name="inconclusive" select="$tests[@id = key('outcome', 'inconclusive')/@id]" />
    <xsl:variable name="ignored" select="$tests[@id = key('state', 'ignored')/@id]" />
    <xsl:variable name="skipped" select="$tests[@id = key('state', 'skipped')/@id]" />
    <xsl:variable name="assertions" select="key('state', 'executed')[@id = $tests/@id]/g:stepRun/g:result/@assertCount" />
    <xsl:variable name="duration" select="/g:report/g:packageRun/g:testRuns/g:testRun[@id = $id]/g:stepRun/g:result/@duration" />
    <li>
      <span>
        <xsl:choose>
          <xsl:when test="count(g:children/g:test) > 0 and g:metadata/g:entry[@key='ComponentKind']/g:value != 'Fixture'">
            <img src="img/Minus.gif" class="minus">
              <xsl:attribute name="id">toggle<xsl:value-of select="$id" /></xsl:attribute>
              <xsl:attribute name="onclick">toggle('<xsl:value-of select="$id" />');</xsl:attribute>
            </img>
          </xsl:when>
          <xsl:otherwise>
            <img src="img/FullStop.gif" />
          </xsl:otherwise>
        </xsl:choose>
        <xsl:choose>
          <xsl:when test="g:metadata/g:entry[@key='ComponentKind']/g:value = 'Fixture'">
            <img src="img/Fixture.png" alt="Fixture icon" />
          </xsl:when>
          <xsl:otherwise>
            <img src="img/Container.png" alt="Container icon" />
          </xsl:otherwise>
        </xsl:choose>
        <a>
          <xsl:attribute name="href">
            <xsl:text>#</xsl:text>
            <xsl:value-of select="@id" />
          </xsl:attribute>
          <xsl:value-of select="@name" />
        </a>
        <table style="display: inline;">
          <tr>
            <td>
              <xsl:call-template name="progressBar">
                <xsl:with-param name="width">100</xsl:with-param>
                <xsl:with-param name="height">10</xsl:with-param>
                <xsl:with-param name="run">
                  <xsl:value-of select="count($run)" />
                </xsl:with-param>
                <xsl:with-param name="passed">
                  <xsl:value-of select="count($passed)" />
                </xsl:with-param>
                <xsl:with-param name="failed">
                  <xsl:value-of select="count($failed)" />
                </xsl:with-param>
                <xsl:with-param name="inconclusive">
                  <xsl:value-of select="count($inconclusive)" />
                </xsl:with-param>
              </xsl:call-template>
            </td>
          </tr>
        </table>
        (<xsl:value-of select="count($passed)" />/<xsl:value-of select="count($failed)" />/<xsl:value-of select="count($inconclusive)" />)
      </span>
      <xsl:if test="count(g:children/g:test) > 0 and g:metadata/g:entry[@key='ComponentKind']/g:value != 'Fixture'">
        <ul>
          <xsl:attribute name="id">list<xsl:value-of select="$id" /></xsl:attribute>
          <xsl:apply-templates select="g:children/g:test" />
        </ul>
      </xsl:if>
    </li>
  </xsl:template>

  <!-- Scale value -->
  <xsl:template name="scale">
    <xsl:param name="origLength" />
    <xsl:param name="targetLength" />
    <xsl:param name="value" />
    <xsl:value-of select="($value div $origLength) * $targetLength" />
  </xsl:template>

  <!-- Progress bar -->
  <xsl:template name="progressBar">
    <xsl:param name="width" />
    <xsl:param name="height" />
    <xsl:param name="run" />
    <xsl:param name="passed" />
    <xsl:param name="failed" />
    <xsl:param name="inconclusive" />
    <div class="progressBar">
      <xsl:attribute name="style">width:<xsl:value-of select="$width" />px; height:<xsl:value-of select="$height" />px;</xsl:attribute>
      <!-- passed -->
      <xsl:if test="$passed > 0">
        <div class="progressPassed">
          <xsl:attribute name="style">height: <xsl:value-of select="$height" />px; width: <xsl:call-template name="scale">
              <xsl:with-param name="origLength" select="$run" />
              <xsl:with-param name="targetLength" select="$width" />
              <xsl:with-param name="value" select="$passed" />
            </xsl:call-template>px;</xsl:attribute>
        </div>
      </xsl:if>
      <!-- failed -->
      <xsl:if test="$failed > 0">
        <div class="progressFailed">
          <xsl:attribute name="style">left:<xsl:call-template name="scale">
              <xsl:with-param name="origLength" select="$run" />
              <xsl:with-param name="targetLength" select="$width" />
              <xsl:with-param name="value" select="$passed" />
            </xsl:call-template>px; height:<xsl:value-of select="$height" />px; width:<xsl:call-template name="scale">
              <xsl:with-param name="origLength" select="$run" />
              <xsl:with-param name="targetLength" select="$width" />
              <xsl:with-param name="value" select="$failed" />
            </xsl:call-template>px;</xsl:attribute>
        </div>
      </xsl:if>
      <!-- inconclusive -->
      <xsl:if test="$inconclusive > 0">
        <div class="progressInconclusive">
          <xsl:attribute name="style">left:<xsl:call-template name="scale">
              <xsl:with-param name="origLength" select="$run" />
              <xsl:with-param name="targetLength" select="$width" />
              <xsl:with-param name="value" select="$passed + $failed" />
            </xsl:call-template>px; height:<xsl:value-of select="$height" />px; width:<xsl:call-template name="scale">
              <xsl:with-param name="origLength" select="$run" />
              <xsl:with-param name="targetLength" select="$width" />
              <xsl:with-param name="value" select="$inconclusive" />
            </xsl:call-template>px;</xsl:attribute>
        </div>
      </xsl:if>
    </div>
  </xsl:template>

  <!-- Pretty print date time values -->
  <xsl:template name="format-datetime">
    <xsl:param name="datetime" />
    <xsl:value-of select="substring($datetime, 12, 8)" />
    (<xsl:value-of select="substring($datetime, 28)" />)
    <xsl:value-of select="substring($datetime, 1, 10)" />
  </xsl:template>
  
  <xsl:template match="g:packageRun/g:testRuns">
    <div id="Details">
      <h2>Details</h2>
      <xsl:apply-templates select="g:testRun" />
    </div>
  </xsl:template>

  <xsl:template match="g:testRun">
    <xsl:apply-templates select="g:stepRun">
      <xsl:with-param name="testId" select="@id" />
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="g:stepRun">
    <xsl:param name="testId" />
    <xsl:variable name="test" select="//g:testModel//g:test[@id=$testId]" />

    <div>
      <xsl:attribute name="id">
        <xsl:value-of select="$testId" />
      </xsl:attribute>
      <xsl:attribute name="stepId">
        <xsl:value-of select="@id" />
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:value-of select="$test/g:metadata/g:entry[@key='ComponentKind']/g:value" />
        <xsl:if test="$test/@isTestCase = 'true' and g:result/@state != 'ignored'">
          <xsl:text> </xsl:text>
          <xsl:value-of select="g:result/@outcome" />
        </xsl:if>
      </xsl:attribute>

      <xsl:choose>
        <xsl:when test="$test/g:metadata/g:entry[@key='ComponentKind']/g:value = 'Assembly'">
          <h3>
            <img src="img/Container.png" alt="Container icon" />
            <xsl:value-of select="@fullName" />
          </h3>
          <b>Duration:</b> <xsl:value-of select="format-number(g:result/@duration, '0.00')" />s
        </xsl:when>
        <xsl:when test="$test/g:metadata/g:entry[@key='ComponentKind']/g:value = 'Fixture'">
          <img src="img/Fixture.png" alt="Fixture icon" />
          <b>
            <xsl:value-of select="@fullName" /> (<xsl:value-of select="format-number(g:result/@duration, '0.00')" />s)
          </b>
          <xsl:if test="count($test/g:metadata/g:entry) > 1">
            <ul>
              <xsl:apply-templates select="$test/g:metadata/g:entry" />
            </ul>
          </xsl:if>
        </xsl:when>
        <xsl:when test="$test/g:metadata/g:entry[@key='ComponentKind']/g:value = 'Test'">
          <img src="img/Test.png" alt="Test icon" />
          <xsl:value-of select="@fullName" />
          (Duration: <xsl:value-of select="g:result/@duration" />, Assertions: <xsl:value-of select="g:result/@assertCount" />)
        </xsl:when>
      </xsl:choose>
    </div>

    <xsl:apply-templates select="g:executionLog/g:streams/g:stream" />

    <xsl:apply-templates select="g:stepRun">
      <xsl:with-param name="testId" select="$testId" />
    </xsl:apply-templates>

  </xsl:template>

  <xsl:template match="//g:test/g:metadata/g:entry">
    <xsl:if test="@key!='ComponentKind'">
      <li>
        <xsl:value-of select="@key" />: <xsl:value-of select="g:value" />
      </li>
    </xsl:if>
  </xsl:template>

  <xsl:template match="g:executionLog/g:streams/g:stream">
    <div class="executionLog">
      <xsl:value-of select="@name" />
      <div>
        <xsl:attribute name="class">
          <xsl:value-of select="@name" />
        </xsl:attribute>
        <xsl:value-of select="g:body/g:contents/g:text" />
      </div>
    </div>
  </xsl:template>

</xsl:stylesheet>