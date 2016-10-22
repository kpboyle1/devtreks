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
    <xsl:if test="(@TME2Name1 != '')">
		  <tr>
        <th>
				  Indicator Property
			  </th>
        <th>
				  Total
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
    <xsl:if test="(@TME2Name1 != '')">
      <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@TME2Name1" />&#xA0;<xsl:value-of select="@TME2Type1"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 1
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange1"/>
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
					  <xsl:value-of select="@TME2Unit1"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange1"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
           <xsl:value-of select="@TME2Q1Unit1"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange1"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange1"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange1"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit1"/>
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
				  <strong><xsl:value-of select="@TME2Name2" />&#xA0;<xsl:value-of select="@TME2Type2"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 2
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange2"/>
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
					  <xsl:value-of select="@TME2Unit2"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange2"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
            <xsl:value-of select="@TME2Q1Unit2"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange2"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange2"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit2"/>
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
				  <strong><xsl:value-of select="@TME2Name3" />&#xA0;<xsl:value-of select="@TME2Type3"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 3
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange3"/>
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
					  <xsl:value-of select="@TME2Unit3"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange3"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
            <xsl:value-of select="@TME2Q1Unit3"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange3"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange3"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange3"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit3"/>
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
				  <strong><xsl:value-of select="@TME2Name4" />&#xA0;<xsl:value-of select="@TME2Type4"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 4
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange4"/>
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
					  <xsl:value-of select="@TME2Unit4"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange4"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
            <xsl:value-of select="@TME2Q1Unit4"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange4"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange4"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange4"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit4"/>
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
				  <strong><xsl:value-of select="@TME2Name5" />&#xA0;<xsl:value-of select="@TME2Type5"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 5
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange5"/>
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
					  <xsl:value-of select="@TME2Unit5"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange5"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q1Unit5"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange5"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange5"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange5"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit5"/>
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
				  <strong><xsl:value-of select="@TME2Name6" />&#xA0;<xsl:value-of select="@TME2Type6"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 6
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange6"/>
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
					  <xsl:value-of select="@TME2Unit6"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange6"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q1Unit6"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange6"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange6"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange6"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit6"/>
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
				  <strong><xsl:value-of select="@TME2Name7" />&#xA0;<xsl:value-of select="@TME2Type7"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 7
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange7"/>
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
					  <xsl:value-of select="@TME2Unit7"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange7"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q1Unit7"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange7"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange7"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange7"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit7"/>
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
				  <strong><xsl:value-of select="@TME2Name8" />&#xA0;<xsl:value-of select="@TME2Type8"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 8
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange8"/>
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
					  <xsl:value-of select="@TME2Unit8"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange8"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q1Unit8"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange8"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange8"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange8"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit8"/>
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
				  <strong><xsl:value-of select="@TME2Name9" />&#xA0;<xsl:value-of select="@TME2Type9"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 9
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange9"/>
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
					  <xsl:value-of select="@TME2Unit9"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange9"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q1Unit9"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange9"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange9"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange9"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit9"/>
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
				  <strong><xsl:value-of select="@TME2Name10" />&#xA0;<xsl:value-of select="@TME2Type10"/></strong>
        </td>
      </tr>
			<tr>
			  <td>
				  Total 10
			  </td>
			  <td>
				  <xsl:value-of select="@TME2Total10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2AmountChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2PercentChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2BaseChange10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2BasePercentChange10"/>
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
					  <xsl:value-of select="@TME2Unit10"/>
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
					  <xsl:value-of select="@TME2Q1AmountChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1PercentChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q1BaseChange10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q1BasePercentChange10"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q1Unit10"/>
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
					  <xsl:value-of select="@TME2Q2AmountChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2PercentChange10"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TME2Q2BaseChange10"/>
			  </td>
        <td>
					  <xsl:value-of select="@TME2Q2BasePercentChange10"/>
			  </td>
        <td>
			  </td>
			  <td>
			  </td>
			  <td>
			  </td>
			  <td>
          <xsl:value-of select="@TME2Q2Unit10"/>
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

