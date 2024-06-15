import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';

// https://astro.build/config
export default defineConfig({
	site: 'https://jean-voila.github.io',
	base: 'sqrt100',
	integrations: [
		starlight({
			customCss: ["/sqrt100/src/styles/main.css"],
			title: 'Castle Of Demise',
			social: {
				github: 'https://github.com/jean-voila/sqrt100',
			},
			sidebar: [
				{
					label: 'Histoire du projet',
					autogenerate: { directory: '/sqrt100/histoire-du-projet' },
				},
				{
					label: 'Téléchargements',
					autogenerate: { directory: '/sqrt100/telechargements' },
				},
			],
		}),
	],
});
