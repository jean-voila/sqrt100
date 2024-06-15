import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';

// https://astro.build/config
export default defineConfig({
	site: 'https://jean-voila.github.io',
	base: 'sqrt100',
	integrations: [
		starlight({
			customCss: ["./src/styles/main.css"],
			title: 'Castle Of Demise',
			social: {
				github: 'https://github.com/jean-voila/sqrt100',
			},
			sidebar: [
				{
					label: 'Histoire du projet',
					autogenerate: { directory: 'histoire-du-projet' },
				},
				{
					label: 'Téléchargements',
					autogenerate: { directory: 'telechargements' },
				},
			],
		}),
	],
	site: "https://docs.jeanflix.fr"
});
