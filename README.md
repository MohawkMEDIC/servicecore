<p><strong>Project Description</strong></p>
<p>The MEDIC Service Core Framework was developed at the Mohawk College mHealth and eHealth Development and Innovation Centre (MEDIC) as part of our five year NSERC CCI grant to prototype electronic health records (EHRs) infrastructure. The Service Core&rsquo;s purpose was to allow the rapid prototyping and reuse of components between EHRs software reference implementations as well as providing a consistent configuration mechanism between these services.</p>
<p>The following projects at the MEDIC leverage the Service Core:</p>
<ul>
<li>The MEDIC Client Registry Reference Implementation</li>
<li>The MEDIC Shared Health Record Reference Implementation</li>
<li>The MEDIC ATNA Visualizer</li>
</ul>
<p>The service core is hosted as a separate project in order to allow community members to better remove pieces of the service core for their own purposes. The Service Core provides the following service implementations:</p>
<ul>
<li>A configuration utility which allows component services to be configured (or configure themselves). An example of this tool is provided in the Client Registry configuration tool</li>
<li>Terminology Resolution
<ul>
<li>HL7 CTS 1.2 client implementation</li>
<li>A datatable (Postgresql) based lookup/mapping service</li>
</ul>
</li>
<li>Clinical Messaging
<ul>
<li>HL7v3 (via Everest) messaging service</li>
<li>HL7 Fast Health Interoperability Resource (FHIR) messaging service</li>
<li>Multi-head messaging service</li>
</ul>
</li>
<li>Localization services based on XML files</li>
<li>Timer service for executing scheduled</li>
<li>Auditing &amp; Logging
<ul>
<li>File based auditing</li>
<li>Rollover trace listeners</li>
<li>ATNA (RFC3881/DICOM) based auditing</li>
</ul>
</li>
<li>Stateful query continuation services</li>
</ul>
<div>
<script type="text/javascript" src="https://jira.marc-hi.ca/s/be0fd92c13691c7c8ffab8223cf7b697-T/qz1074/73012/9c49254d0b8026d9562344f6e652bde4/2.0.23/_/download/batch/com.atlassian.jira.collector.plugin.jira-issue-collector-plugin:issuecollector/com.atlassian.jira.collector.plugin.jira-issue-collector-plugin:issuecollector.js?locale=en-US&collectorId=97dd5319"></script>
  </div>


