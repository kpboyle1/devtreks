<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Investment"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8" />
	<!-- pass in params -->
	<!-- what action is being taken by the server -->
	<xsl:param name="serverActionType" />
	<!-- what other action is being taken by the server -->
	<xsl:param name="serverSubActionType" />
	<!-- is the member viewing this uri the owner? -->
	<xsl:param name="isURIOwningClub" />
	<!-- which node to start with? -->
	<xsl:param name="nodeName" />
	<!-- which view to use? -->
	<xsl:param name="viewEditType" />
	<!-- is this a coordinator? -->
	<xsl:param name="memberRole" />
	<!-- what is the current uri? -->
	<xsl:param name="selectedFileURIPattern" />
	<!-- the addin being used -->
	<xsl:param name="calcDocURI" />
	<!-- the node being calculated-->
	<xsl:param name="docToCalcNodeName" />
	<!-- standard params used with calcs and custom docs -->
	<xsl:param name="calcParams" />
	<!-- what is the name of the node to be selected? -->
	<xsl:param name="selectionsNodeNeededName" />
	<!-- which network is this doc from? -->
	<xsl:param name="networkId" />
	<!-- what is the start row? -->
	<xsl:param name="startRow" />
	<!-- what is the end row? -->
	<xsl:param name="endRow" />
	<!-- what are the pagination properties ? -->
	<xsl:param name="pageParams" />
	<!-- what is the guide's email? -->
	<xsl:param name="clubEmail" />
	<!-- what is the current serviceid? -->
	<xsl:param name="contenturipattern" />
	<!-- path to resources -->
	<xsl:param name="fullFilePath" />
	<!-- init html -->
	<xsl:template match="@*|/|node()" />
	<xsl:template match="/">
		<xsl:apply-templates select="root" />
	</xsl:template>
	<xsl:template match="root">
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="investmentgroup" />
					<tr id="footer">
						<td scope="row" colspan="10">
							<a id="aFeedback" name="Feedback">
								<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
								Feedback About <xsl:value-of select="$selectedFileURIPattern" />
							</a>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="investmentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentgroup">
    <tr>
			<th scope="col" colspan="10">
				Investment Group :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<tr>
			<th scope="col" colspan="10">
				Investment :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<tr>
			<th scope="col" colspan="10">
				Time Period :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
			</th>
		</tr>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="outcount" select="count(investmentoutcomes/investmentoutcome)"/>
    <xsl:if test="($outcount > 0)"> 
		  <xsl:apply-templates select="investmentoutcomes" />
    </xsl:if>
    <xsl:variable name="opcount" select="count(investmentcomponents/investmentcomponent)"/>
    <xsl:if test="($opcount > 0)"> 
		  <xsl:apply-templates select="investmentcomponents" />
    </xsl:if>
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
		<tr>
			<td scope="row" colspan="10">
				<strong>Outcome:&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	<xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Output:&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponents">
		<xsl:apply-templates select="investmentcomponent">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponent">
		<tr>
			<td scope="row" colspan="10">
				<strong>Component&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	<xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="(@TargetType != '' and @TargetType != 'none')">
      <tr>
			  <td scope="row" colspan="10">
          Target Type: <strong><xsl:value-of select="@TargetType"/></strong>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name1 != '')">
		  <tr>
        <th>
          Indicator Property
        </th>
        <th>
          Plan Period
        </th>
        <th>
          Plan Full
        </th>
        <th>
          Plan Cumul
        </th>
        <th>
          Actual Period
        </th>
        <th>
          Actual Cumul
        </th>
        <th>
          Actual Period Progress
        </th>
        <th>
          Actual Cumul Progress
        </th>
        <th>
          Plan P Percent ; Plan C Percent
        </th>
        <th>
          Plan Full Percent
        </th>
      </tr>
     </xsl:if>
    <xsl:if test="(@TME2Name1 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name1" />&#xA0;<xsl:value-of select="@TME2Label1"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date1"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N1"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit1"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit1"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit1"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total1"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent1"/> ; <xsl:value-of select="@TPCPercent1"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent1"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent1"/> ; <xsl:value-of select="@TQ1PCPercent1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent1"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent1"/> ; <xsl:value-of select="@TQ2PCPercent1"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent1"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description1" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name2 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name2" />&#xA0;<xsl:value-of select="@TME2Label2"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date2"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N2"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit2"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit2"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit2"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total2"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent2"/> ; <xsl:value-of select="@TPCPercent2"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent2"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent2"/> ; <xsl:value-of select="@TQ1PCPercent2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent2"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent2"/> ; <xsl:value-of select="@TQ2PCPercent2"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent2"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description2" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name3 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name3" />&#xA0;<xsl:value-of select="@TME2Label3"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date3"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N3"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit3"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit3"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit3"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total3"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent3"/> ; <xsl:value-of select="@TPCPercent3"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent3"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent3"/> ; <xsl:value-of select="@TQ1PCPercent3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent3"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent3"/> ; <xsl:value-of select="@TQ2PCPercent3"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent3"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description3" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name4 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name4" />&#xA0;<xsl:value-of select="@TME2Label4"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date4"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N4"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit4"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit4"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit4"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total4"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent4"/> ; <xsl:value-of select="@TPCPercent4"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent4"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent4"/> ; <xsl:value-of select="@TQ1PCPercent4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent4"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent4"/> ; <xsl:value-of select="@TQ2PCPercent4"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent4"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description4" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name5 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name5" />&#xA0;<xsl:value-of select="@TME2Label5"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date5"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N5"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit5"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit5"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit5"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total5"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent5"/> ; <xsl:value-of select="@TPCPercent5"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent5"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent5"/> ; <xsl:value-of select="@TQ1PCPercent5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent5"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent5"/> ; <xsl:value-of select="@TQ2PCPercent5"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent5"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description5" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name6 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name6" />&#xA0;<xsl:value-of select="@TME2Label6"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date6"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N6"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit6"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit6"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit6"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total6"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent6"/> ; <xsl:value-of select="@TPCPercent6"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent6"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent6"/> ; <xsl:value-of select="@TQ1PCPercent6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent6"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent6"/> ; <xsl:value-of select="@TQ2PCPercent6"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent6"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description6" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name7 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name7" />&#xA0;<xsl:value-of select="@TME2Label7"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date7"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N7"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit7"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit7"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit7"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total7"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent7"/> ; <xsl:value-of select="@TPCPercent7"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent7"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent7"/> ; <xsl:value-of select="@TQ1PCPercent7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent7"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent7"/> ; <xsl:value-of select="@TQ2PCPercent7"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent7"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description7" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name8 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name8" />&#xA0;<xsl:value-of select="@TME2Label8"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date8"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N8"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit8"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit8"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit8"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total8"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent8"/> ; <xsl:value-of select="@TPCPercent8"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent8"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent8"/> ; <xsl:value-of select="@TQ1PCPercent8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent8"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent8"/> ; <xsl:value-of select="@TQ2PCPercent8"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent8"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description8" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name9 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name9" />&#xA0;<xsl:value-of select="@TME2Label9"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date9"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N9"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit9"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit9"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit9"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total9"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent9"/> ; <xsl:value-of select="@TPCPercent9"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent9"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent9"/> ; <xsl:value-of select="@TQ1PCPercent9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent9"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent9"/> ; <xsl:value-of select="@TQ2PCPercent9"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent9"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description9" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name10 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name10" />&#xA0;<xsl:value-of select="@TME2Label10"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date10"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N10"/>
           <br />
           Total Unit:&#xA0;<xsl:value-of select="@TME2Unit10"/>;&#xA0;Q1 Unit:&#xA0;<xsl:value-of select="@TME2Q1Unit10"/>;&#xA0;Q2 Unit:&#xA0;<xsl:value-of select="@TME2Q2Unit10"/>
        </td>
      </tr>
			<tr>
        <td>
          Total
        </td>
        <td>
          <xsl:value-of select="@TME2Total10"/>
        </td>
        <td>
          <xsl:value-of select="@TPFTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TPCTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TAPTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TACTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TAPChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TACChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TPPPercent10"/> ; <xsl:value-of select="@TPCPercent10"/>
        </td>
        <td>
          <xsl:value-of select="@TPFPercent10"/>
        </td>
      </tr>
      <tr>
        <td>
          Q1
        </td>
        <td>
          <xsl:value-of select="@TME2Q1Total10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PCTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1APChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1ACChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PPPercent10"/> ; <xsl:value-of select="@TQ1PCPercent10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ1PFPercent10"/>
        </td>
      </tr>
      <tr>
        <td>
          Q2
        </td>
        <td>
          <xsl:value-of select="@TME2Q2Total10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PCTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2APChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2ACChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PPPercent10"/> ; <xsl:value-of select="@TQ2PCPercent10"/>
        </td>
        <td>
          <xsl:value-of select="@TQ2PFPercent10"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description10" />
			  </td>
		  </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>

