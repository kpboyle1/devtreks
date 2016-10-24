<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Output"
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
					<xsl:apply-templates select="outputgroup" />
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
		<xsl:apply-templates select="outputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputgroup">
		<tr>
			<th scope="col" colspan="10">
				Output Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="output">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
    <tr>
			<th scope="col" colspan="10"><strong>Output</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outputseries">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputseries">
    <tr>
			<th scope="col" colspan="10"><strong>Output Series</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
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
    <xsl:if test="(@TME2Name1 != '' and @TME2Name1 != 'none')">
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
    <xsl:if test="(@TME2Name0 != '' and @TME2Name0 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name0" />&#xA0;<xsl:value-of select="@TME2Label0"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date0"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N0"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit0"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit0"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit0"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount0"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange0"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange0"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent0"/> ; <xsl:value-of select="@TMPCPercent0"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent0"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount0"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange0"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange0"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent0"/> ; <xsl:value-of select="@TLPCPercent0"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent0"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount0"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal0"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange0"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange0"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent0"/> ; <xsl:value-of select="@TUPCPercent0"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent0"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description0" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name1 != '' and @TME2Name1 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name1" />&#xA0;<xsl:value-of select="@TME2Label1"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date1"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N1"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit1"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit1"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit1"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount1"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent1"/> ; <xsl:value-of select="@TMPCPercent1"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent1"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount1"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent1"/> ; <xsl:value-of select="@TLPCPercent1"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent1"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount1"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal1"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange1"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent1"/> ; <xsl:value-of select="@TUPCPercent1"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent1"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description1" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name2 != '' and @TME2Name2 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name2" />&#xA0;<xsl:value-of select="@TME2Label2"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date2"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N2"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit2"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit2"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit2"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount2"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent2"/> ; <xsl:value-of select="@TMPCPercent2"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent2"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount2"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent2"/> ; <xsl:value-of select="@TLPCPercent2"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent2"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount2"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal2"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange2"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent2"/> ; <xsl:value-of select="@TUPCPercent2"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent2"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description2" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name3 != '' and @TME2Name3 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name3" />&#xA0;<xsl:value-of select="@TME2Label3"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date3"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N3"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit3"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit3"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit3"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount3"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent3"/> ; <xsl:value-of select="@TMPCPercent3"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent3"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount3"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent3"/> ; <xsl:value-of select="@TLPCPercent3"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent3"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount3"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal3"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange3"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent3"/> ; <xsl:value-of select="@TUPCPercent3"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent3"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description3" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name4 != '' and @TME2Name4 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name4" />&#xA0;<xsl:value-of select="@TME2Label4"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date4"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N4"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit4"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit4"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit4"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount4"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent4"/> ; <xsl:value-of select="@TMPCPercent4"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent4"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount4"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent4"/> ; <xsl:value-of select="@TLPCPercent4"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent4"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount4"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal4"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange4"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent4"/> ; <xsl:value-of select="@TUPCPercent4"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent4"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description4" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name5 != '' and @TME2Name5 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name5" />&#xA0;<xsl:value-of select="@TME2Label5"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date5"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N5"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit5"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit5"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit5"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount5"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent5"/> ; <xsl:value-of select="@TMPCPercent5"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent5"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount5"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent5"/> ; <xsl:value-of select="@TLPCPercent5"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent5"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount5"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal5"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange5"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent5"/> ; <xsl:value-of select="@TUPCPercent5"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent5"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description5" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name6 != '' and @TME2Name6 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name6" />&#xA0;<xsl:value-of select="@TME2Label6"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date6"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N6"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit6"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit6"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit6"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount6"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent6"/> ; <xsl:value-of select="@TMPCPercent6"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent6"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount6"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent6"/> ; <xsl:value-of select="@TLPCPercent6"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent6"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount6"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal6"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange6"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent6"/> ; <xsl:value-of select="@TUPCPercent6"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent6"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description6" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name7 != '' and @TME2Name7 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name7" />&#xA0;<xsl:value-of select="@TME2Label7"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date7"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N7"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit7"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit7"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit7"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount7"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent7"/> ; <xsl:value-of select="@TMPCPercent7"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent7"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount7"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent7"/> ; <xsl:value-of select="@TLPCPercent7"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent7"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount7"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal7"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange7"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent7"/> ; <xsl:value-of select="@TUPCPercent7"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent7"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description7" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name8 != '' and @TME2Name8 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name8" />&#xA0;<xsl:value-of select="@TME2Label8"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date8"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N8"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit8"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit8"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit8"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount8"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent8"/> ; <xsl:value-of select="@TMPCPercent8"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent8"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount8"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent8"/> ; <xsl:value-of select="@TLPCPercent8"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent8"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount8"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal8"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange8"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent8"/> ; <xsl:value-of select="@TUPCPercent8"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent8"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description8" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name9 != '' and @TME2Name9 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name9" />&#xA0;<xsl:value-of select="@TME2Label9"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date9"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N9"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit9"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit9"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit9"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount9"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent9"/> ; <xsl:value-of select="@TMPCPercent9"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent9"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount9"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent9"/> ; <xsl:value-of select="@TLPCPercent9"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent9"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount9"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal9"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange9"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent9"/> ; <xsl:value-of select="@TUPCPercent9"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent9"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description9" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name10 != '' and @TME2Name10 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name10" />&#xA0;<xsl:value-of select="@TME2Label10"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date10"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N10"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit10"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit10"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit10"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount10"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent10"/> ; <xsl:value-of select="@TMPCPercent10"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent10"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount10"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent10"/> ; <xsl:value-of select="@TLPCPercent10"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent10"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount10"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal10"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange10"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent10"/> ; <xsl:value-of select="@TUPCPercent10"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent10"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description10" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name11 != '' and @TME2Name11 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name11" />&#xA0;<xsl:value-of select="@TME2Label11"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date11"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N11"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit11"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit11"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit11"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount11"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange11"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange11"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent11"/> ; <xsl:value-of select="@TMPCPercent11"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent11"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount11"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange11"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange11"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent11"/> ; <xsl:value-of select="@TLPCPercent11"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent11"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount11"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal11"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange11"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange11"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent11"/> ; <xsl:value-of select="@TUPCPercent11"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent11"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description11" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name12 != '' and @TME2Name12 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name12" />&#xA0;<xsl:value-of select="@TME2Label12"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date12"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N12"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit12"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit12"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit12"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount12"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange12"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange12"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent12"/> ; <xsl:value-of select="@TMPCPercent12"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent12"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount12"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange12"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange12"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent12"/> ; <xsl:value-of select="@TLPCPercent12"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent12"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount12"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal12"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange12"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange12"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent12"/> ; <xsl:value-of select="@TUPCPercent12"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent12"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description12" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name13 != '' and @TME2Name13 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name13" />&#xA0;<xsl:value-of select="@TME2Label13"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date13"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N13"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit13"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit13"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit13"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount13"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange13"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange13"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent13"/> ; <xsl:value-of select="@TMPCPercent13"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent13"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount13"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange13"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange13"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent13"/> ; <xsl:value-of select="@TLPCPercent13"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent13"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount13"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal13"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange13"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange13"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent13"/> ; <xsl:value-of select="@TUPCPercent13"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent13"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description13" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name14 != '' and @TME2Name14 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name14" />&#xA0;<xsl:value-of select="@TME2Label14"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date14"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N14"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit14"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit14"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit14"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount14"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange14"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange14"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent14"/> ; <xsl:value-of select="@TMPCPercent14"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent14"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount14"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange14"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange14"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent14"/> ; <xsl:value-of select="@TLPCPercent14"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent14"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount14"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal14"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange14"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange14"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent14"/> ; <xsl:value-of select="@TUPCPercent14"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent14"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description14" />
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name15 != '' and @TME2Name15 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name15" />&#xA0;<xsl:value-of select="@TME2Label15"/></strong>
           <br />
           Date:&#xA0;<xsl:value-of select="@TME2Date15"/>;&#xA0;Observations:&#xA0;<xsl:value-of select="@TME2N15"/>
           <br />
           Most Unit:&#xA0;<xsl:value-of select="@TME2TMUnit15"/>;&#xA0;L Unit:&#xA0;<xsl:value-of select="@TME2TLUnit15"/>;&#xA0;U Unit:&#xA0;<xsl:value-of select="@TME2TUUnit15"/>
        </td>
      </tr>
			<tr>
        <td>
          Most Likely
        </td>
        <td>
          <xsl:value-of select="@TME2TMAmount15"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TMPCTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TMACTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TMAPChange15"/>
        </td>
        <td>
          <xsl:value-of select="@TMACChange15"/>
        </td>
        <td>
          <xsl:value-of select="@TMPPPercent15"/> ; <xsl:value-of select="@TMPCPercent15"/>
        </td>
        <td>
          <xsl:value-of select="@TMPFPercent15"/>
        </td>
      </tr>
      <tr>
        <td>
          L
        </td>
        <td>
          <xsl:value-of select="@TME2TLAmount15"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TLPCTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TLACTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TLAPChange15"/>
        </td>
        <td>
          <xsl:value-of select="@TLACChange15"/>
        </td>
        <td>
          <xsl:value-of select="@TLPPPercent15"/> ; <xsl:value-of select="@TLPCPercent15"/>
        </td>
        <td>
          <xsl:value-of select="@TLPFPercent15"/>
        </td>
      </tr>
      <tr>
        <td>
          U
        </td>
        <td>
          <xsl:value-of select="@TME2TUAmount15"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TUPCTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TUACTotal15"/>
        </td>
        <td>
          <xsl:value-of select="@TUAPChange15"/>
        </td>
        <td>
          <xsl:value-of select="@TUACChange15"/>
        </td>
        <td>
          <xsl:value-of select="@TUPPPercent15"/> ; <xsl:value-of select="@TUPCPercent15"/>
        </td>
        <td>
          <xsl:value-of select="@TUPFPercent15"/>
        </td>
      </tr>
      <tr>
			  <td scope="row" colspan="10">
				  <xsl:value-of select="@TME2Description15" />
			  </td>
		  </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>

