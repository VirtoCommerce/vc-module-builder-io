# Virto Commerce Builder.io Integration Module

Builder.io module allows you to enable Builder.io and assign Public API Key for a Store and give your developers
and marketers the toolkit to transform page designs into optimized web and mobile experiences.

## Key Features
1. Store Configuration.
1. Integarted with Virto Storefront.
1. Ready for integration with other sales channels.
1. Application menu.

## Screenshots
![image](https://github.com/VirtoCommerce/vc-module-builder-io/assets/7639413/a93494be-1514-4189-91ea-f930f583aa9b)

![image](https://github.com/VirtoCommerce/vc-module-builder-io/assets/7639413/c642e205-6884-414d-9de1-9f42084aeb30)

## Setup
1. [Create and сonfigure Builder.io account](https://www.builder.io/)
1. Go to a Store Settings, select Settings and activate Builder.io and enter your Public API Key.

## Integration with Virto Frontend
Virto Frontend and Vue B2B Theme has native integration with Builder.io module. 
Once you click Save for Store Settings, the Builder.io tracking will be activated.


## Application Menu 
The module adds `Builder.io` link into Application menu. It redirects to Builder.io dashboard. 

![image](https://github.com/VirtoCommerce/vc-module-builder-io/assets/7639413/88565cf4-bcfc-451a-a555-301e98e73d2c)


## Pages Module Integration

The module integrates with [Virto Pages](https://github.com/VirtoCommerce/vc-module-pages) as a content provider (`IPageContentProvider`), enabling:

* **Index Rebuild** - full reindex of all Builder.io pages from the admin UI
* **Scheduled Sync** - periodic synchronization of modified pages using the `lastUpdated` filter
* **Webhook Push** - real-time page updates via `POST /api/pages/builder-io` (existing functionality)

The content provider uses the [Builder.io Content API](https://www.builder.io/c/docs/content-api) (`GET https://cdn.builder.io/api/v3/content/page`) with the store's `PublicApiKey` setting to query pages. It supports pagination (`limit`/`offset`), date filtering (`query.lastUpdated.$gte`), and bulk retrieval by ID (`query.id.$in`).

### Required Targeting Attributes

For index rebuild and scheduled sync to work correctly, Builder.io page models should include the following targeting attributes (Query properties):

* **`storeId`** — the Virto Commerce store ID this page belongs to
* **`locale`** — the culture/language code (e.g., `en-US`)

These attributes are read directly from the page model during reindexation. When pages arrive via webhook, HTTP header values are used as a fallback if the model does not contain these attributes.

## Documentation

* [Builder.io](https://www.builder.io)
* [Builder.io Content API](https://www.builder.io/c/docs/content-api)
* [Builder.io Querying Cheatsheet](https://www.builder.io/c/docs/querying)
* [Builder.io SDK source](https://github.com/BuilderIO/builder/blob/main/packages/core/src/builder.class.ts)
* [Builder.io module user documentation](https://docs.virtocommerce.org/platform/user-guide/integrations/builder-io/overview/)
* [REST API](https://virtostart-demo-admin.govirto.com/docs/index.html?urls.primaryName=VirtoCommerce.BuilderIO)
* [View on GitHub](https://github.com/VirtoCommerce/vc-module-builder-io)


## References

* [Deployment](https://docs.virtocommerce.org/platform/developer-guide/Tutorials-and-How-tos/Tutorials/deploy-module-from-source-code/)
* [Installation](https://docs.virtocommerce.org/platform/user-guide/modules-installation/)
* [Home](https://virtocommerce.com)
* [Community](https://www.virtocommerce.org)
* [Download latest release](https://github.com/VirtoCommerce/vc-module-builder-io/releases/latest)


## License
Copyright (c) Virto Solutions LTD.  All rights reserved.

This software is licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at http://virtocommerce.com/opensourcelicense.

Unless required by the applicable law or agreed to in written form, the software
distributed under the License is provided on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.
