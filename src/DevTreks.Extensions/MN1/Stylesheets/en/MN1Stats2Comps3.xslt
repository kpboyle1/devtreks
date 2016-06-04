<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Component"
	xmlns:DisplayComps="urn:displaycomps">
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
					<xsl:apply-templates select="componentgroup" />
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
		<xsl:apply-templates select="componentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="componentgroup">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
      <xsl:variable name="title">Component Group : <xsl:value-of select="@Name" /> ; <xsl:value-of select="@Num_0" /></xsl:variable>
      <xsl:value-of select="DisplayComps:writeStringFullColumnTH($title, $fullcolcount)"/>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="component">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="component">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
			<th scope="col">
				Component
			</th>
			<xsl:value-of select="DisplayComps:writeTitles($fullcolcount)"/>
		</tr>
    <tr>
			<td scope="row" colspan="1"><strong>Name</strong></td>
			<td>
				<!--Total-->
			</td>
			<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			<xsl:for-each select="@*">
				<xsl:variable name="att_name" select="name()"/>
				<xsl:variable name="att_value" select="."/>
				<xsl:value-of select="DisplayComps:printValue('Name_', $att_name, $att_value)"/> 
			</xsl:for-each>
			<xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		</tr>
    <tr>
			<td scope="row" colspan="1"><strong>Label</strong></td>
			<td>
			</td>
			<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			<xsl:for-each select="@*">
				<xsl:variable name="att_name" select="name()"/>
				<xsl:variable name="att_value" select="."/>
				<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
			</xsl:for-each>
			<xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
      <tr>
			  <td scope="row" colspan="1"><strong>Observations</strong></td>
			  <td>
				  <xsl:value-of select="@TQN"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TQN_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Total </strong></td>
			  <td>
				  <xsl:value-of select="@TMN1Q"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN1Q_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Name</strong></td>
			  <td>
				  <xsl:value-of select="@TMN1Name"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN1Name_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Mean</strong></td>
			  <td>
				  <xsl:value-of select="@TMN1Mean"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN1Mean_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Median</strong></td>
			  <td>
				  <xsl:value-of select="@TMN1Median"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN1Median_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Variance</strong></td>
			  <td>
				  <xsl:value-of select="@TMN1Variance"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN1Variance_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			  <td>
				  <xsl:value-of select="@TMN1StandDev"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN1StandDev_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Name</strong></td>
			  <td>
				  <xsl:value-of select="@TMN2Name"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN2Name_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Total </strong></td>
			  <td>
				  <xsl:value-of select="@TMN2Q"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN2Q_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Mean</strong></td>
			  <td>
				  <xsl:value-of select="@TMN2Mean"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN2Mean_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Median</strong></td>
			  <td>
				  <xsl:value-of select="@TMN2Median"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN2Median_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Variance</strong></td>
			  <td>
				  <xsl:value-of select="@TMN2Variance"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN2Variance_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			  <td>
				  <xsl:value-of select="@TMN2StandDev"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TMN2StandDev_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <xsl:if test="(@FNCount > 2)">
        <tr>
			    <td scope="row" colspan="1"><strong>Name</strong></td>
			    <td>
				    <xsl:value-of select="@TMN3Name"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN3Name_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Total </strong></td>
			    <td>
				    <xsl:value-of select="@TMN3Q"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN3Q_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Mean</strong></td>
			    <td>
				    <xsl:value-of select="@TMN3Mean"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN3Mean_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Median</strong></td>
			    <td>
				    <xsl:value-of select="@TMN3Median"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN3Median_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Variance</strong></td>
			    <td>
				    <xsl:value-of select="@TMN3Variance"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN3Variance_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			    <td>
				    <xsl:value-of select="@TMN3StandDev"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN3StandDev_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
      </xsl:if>
      <xsl:if test="(@FNCount > 3)">
        <tr>
			    <td scope="row" colspan="1"><strong>Name</strong></td>
			    <td>
				    <xsl:value-of select="@TMN4Name"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN4Name_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Total </strong></td>
			    <td>
				    <xsl:value-of select="@TMN4Q"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN4Q_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Mean</strong></td>
			    <td>
				    <xsl:value-of select="@TMN4Mean"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN4Mean_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Median</strong></td>
			    <td>
				    <xsl:value-of select="@TMN4Median"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN4Median_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Variance</strong></td>
			    <td>
				    <xsl:value-of select="@TMN4Variance"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN4Variance_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			    <td>
				    <xsl:value-of select="@TMN4StandDev"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN4StandDev_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
      </xsl:if>
      <xsl:if test="(@FNCount > 4)">
        <tr>
			    <td scope="row" colspan="1"><strong>Name</strong></td>
			    <td>
				    <xsl:value-of select="@TMN5Name"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN5Name_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Total </strong></td>
			    <td>
				    <xsl:value-of select="@TMN5Q"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN5Q_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Mean</strong></td>
			    <td>
				    <xsl:value-of select="@TMN5Mean"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN5Mean_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Median</strong></td>
			    <td>
				    <xsl:value-of select="@TMN5Median"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN5Median_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Variance</strong></td>
			    <td>
				    <xsl:value-of select="@TMN5Variance"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN5Variance_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			    <td>
				    <xsl:value-of select="@TMN5StandDev"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN5StandDev_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
      </xsl:if>
      <xsl:if test="(@FNCount > 5)">
        <tr>
			    <td scope="row" colspan="1"><strong>Name</strong></td>
			    <td>
				    <xsl:value-of select="@TMN6Name"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN6Name_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Total </strong></td>
			    <td>
				    <xsl:value-of select="@TMN6Q"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN6Q_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Mean</strong></td>
			    <td>
				    <xsl:value-of select="@TMN6Mean"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN6Mean_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Median</strong></td>
			    <td>
				    <xsl:value-of select="@TMN6Median"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN6Median_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Variance</strong></td>
			    <td>
				    <xsl:value-of select="@TMN6Variance"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN6Variance_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			    <td>
				    <xsl:value-of select="@TMN6StandDev"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN6StandDev_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
      </xsl:if>
      <xsl:if test="(@FNCount > 6)">
        <tr>
			    <td scope="row" colspan="1"><strong>Name</strong></td>
			    <td>
				    <xsl:value-of select="@TMN7Name"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN7Name_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Total </strong></td>
			    <td>
				    <xsl:value-of select="@TMN7Q"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN7Q_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Mean</strong></td>
			    <td>
				    <xsl:value-of select="@TMN7Mean"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN7Mean_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Median</strong></td>
			    <td>
				    <xsl:value-of select="@TMN7Median"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN7Median_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Variance</strong></td>
			    <td>
				    <xsl:value-of select="@TMN7Variance"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN7Variance_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			    <td>
				    <xsl:value-of select="@TMN7StandDev"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN7StandDev_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
      </xsl:if>
      <xsl:if test="(@FNCount > 7)">
        <tr>
			    <td scope="row" colspan="1"><strong>Name</strong></td>
			    <td>
				    <xsl:value-of select="@TMN8Name"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN8Name_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Total </strong></td>
			    <td>
				    <xsl:value-of select="@TMN8Q"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN8Q_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Mean</strong></td>
			    <td>
				    <xsl:value-of select="@TMN8Mean"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN8Mean_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Median</strong></td>
			    <td>
				    <xsl:value-of select="@TMN8Median"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN8Median_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Variance</strong></td>
			    <td>
				    <xsl:value-of select="@TMN8Variance"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN8Variance_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			    <td>
				    <xsl:value-of select="@TMN8StandDev"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN8StandDev_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
      </xsl:if>
      <xsl:if test="(@FNCount > 8)">
        <tr>
			    <td scope="row" colspan="1"><strong>Name</strong></td>
			    <td>
				    <xsl:value-of select="@TMN9Name"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN9Name_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Total </strong></td>
			    <td>
				    <xsl:value-of select="@TMN9Q"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN9Q_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Mean</strong></td>
			    <td>
				    <xsl:value-of select="@TMN9Mean"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN9Mean_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Median</strong></td>
			    <td>
				    <xsl:value-of select="@TMN9Median"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN9Median_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Variance</strong></td>
			    <td>
				    <xsl:value-of select="@TMN9Variance"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN9Variance_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			    <td>
				    <xsl:value-of select="@TMN9StandDev"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN9StandDev_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
      </xsl:if>
      <xsl:if test="(@FNCount > 9)">
        <tr>
			    <td scope="row" colspan="1"><strong>Name</strong></td>
			    <td>
				    <xsl:value-of select="@TMN10Name"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN10Name_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Total </strong></td>
			    <td>
				    <xsl:value-of select="@TMN10Q"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN10Q_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Mean</strong></td>
			    <td>
				    <xsl:value-of select="@TMN10Mean"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN10Mean_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Median</strong></td>
			    <td>
				    <xsl:value-of select="@TMN10Median"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN10Median_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Variance</strong></td>
			    <td>
				    <xsl:value-of select="@TMN10Variance"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN10Variance_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
        <tr>
			    <td scope="row" colspan="1"><strong>Std Dev </strong></td>
			    <td>
				    <xsl:value-of select="@TMN10StandDev"/>
			    </td>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
					  <xsl:value-of select="DisplayComps:printValue('TMN10StandDev_', $att_name, $att_value)"/> 
			    </xsl:for-each>
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		    </tr>
      </xsl:if>
	</xsl:template>
</xsl:stylesheet>
