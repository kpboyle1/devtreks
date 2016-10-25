<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Input"
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
					<xsl:apply-templates select="inputgroup" />
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
		<xsl:apply-templates select="inputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="inputgroup">
		<tr>
			<th scope="col" colspan="10">
				Input Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mechangeyr' 
      or @AnalyzerType='mechangeid' or @AnalyzerType='mechangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="input">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="input">
    <tr>
			<th scope="col" colspan="10"><strong>Input</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mechangeyr' 
      or @AnalyzerType='mechangeid' or @AnalyzerType='mechangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="inputseries">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
  <xsl:template match="inputseries">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input Series: <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mechangeyr' 
      or @AnalyzerType='mechangeid' or @AnalyzerType='mechangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="(@AlternativeType != '' and @AlternativeType != 'none')">
      <tr>
			  <td scope="row" colspan="10">
          Alternative Type: <strong><xsl:value-of select="@AlternativeType"/></strong>
			  </td>
		  </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name1 != '' and @TME2Name1 != 'none')">
		  <tr>
        <th>
				  Indicator Property
			  </th>
        <th>
				  Most Likely
			  </th>
			  <th>
				  Amount Change
			  </th>
			  <th>
				  Percent Change
			  </th>
        <th>
				  Base Change
			  </th>
			  <th>
				  Base Percent Change
			  </th>
        <th>
				  Label
			  </th>
        <th>
				  Date
			  </th>
			  <th>
				  Observations
			  </th>
			  <th>
				  Unit
			  </th>
	     </tr>
    </xsl:if>
    <xsl:if test="(@TME2Name0 != '' and @TME2Name0 != 'none')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name0" />&#xA0;<xsl:value-of select="@TME2Label0"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 0
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange0"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label0"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date0"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit0"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange0"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit0"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange0"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange0"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange0"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit0"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange1"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label1"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date1"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit1"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange1"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
           <xsl:value-of select="@TME2TLUnit1"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange1"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit1"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 2
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange2"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label2"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date2"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit2"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange2"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
            <xsl:value-of select="@TME2TLUnit2"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange2"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit2"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 3
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange3"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label3"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date3"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit3"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange3"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
            <xsl:value-of select="@TME2TLUnit3"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange3"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit3"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 4
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange4"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label4"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date4"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit4"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange4"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
            <xsl:value-of select="@TME2TLUnit4"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange4"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit4"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 5
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange5"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label5"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date5"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit5"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange5"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit5"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange5"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit5"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 6
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange6"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label6"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date6"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit6"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange6"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit6"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange6"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit6"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 7
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange7"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label7"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date7"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit7"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange7"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit7"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange7"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit7"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 8
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange8"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label8"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date8"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit8"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange8"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit8"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange8"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit8"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 9
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange9"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label9"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date9"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit9"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange9"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit9"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange9"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit9"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 10
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange10"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label10"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date10"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit10"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange10"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit10"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange10"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit10"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 11
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange11"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label11"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date11"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit11"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange11"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit11"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange11"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange11"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange11"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit11"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 12
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange12"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label12"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date12"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit12"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange12"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit12"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange12"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange12"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange12"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit12"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 13
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange13"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label13"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date13"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit13"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange13"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit13"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange13"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange13"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange13"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit13"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 14
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange14"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label14"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date14"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit14"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange14"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit14"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange14"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange14"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange14"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit14"/>
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
        </td>
      </tr>
			<tr>
			  <td>
				  Most Likely 15
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TMAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MAmountChange15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MPercentChange15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2MBaseChange15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2MBasePercentChange15"/>
			  </td>
        <td>
				  <xsl:value-of select="@TME2Label15"/>
			  </td>
			  <td>
					 <xsl:value-of select="@TME2Date15"/> 
			  </td>
			  <td>
					  <xsl:value-of select="@TME2N15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2TMUnit15"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Lower
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TLAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LAmountChange15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LPercentChange15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2LBaseChange15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2LBasePercentChange15"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TLUnit15"/>
			  </td>
		  </tr>
      <tr>
			  <td>
				  Upper
			  </td>
			  <td>
				  <xsl:value-of select="@TME2TUAmount15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UAmountChange15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UPercentChange15"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2UBaseChange15"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2UBasePercentChange15"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2TUUnit15"/>
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

