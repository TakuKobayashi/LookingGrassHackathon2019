<template>
  <v-layout
    column
    justify-center
    align-center
  >
    <v-flex
      xs12
      sm8
      md6
    >
      <div class="text-center" v-for="item in items" :key="item.value">
        <item :name="item.name" :description="item.description" :image_url="item.image_url" :value="item.value" :currentFlag="val == item.value" />
      </div>

    </v-flex>
  </v-layout>
</template>

<script>
import Item from '~/components/Item.vue'

export default {
  components: {
    Item,

  },
  data: () => ({
    items:[
      {
        name: "鰯",
        description: "えいようまんてん。 旬は初夏から秋口まで。なめろうにして日本酒でおっかけるとうまい",
        image_url: "https://jbpress.ismedia.jp/mwimgs/1/a/600/img_1a8bcc126c28953c6ced3fbd42a6baf6322397.jpg",
        value: "a"
      },
      {
        name: "林檎",
        description: "まっかなりんご。 ",
        image_url: "https://www.aomori-ringo.or.jp/wp-content/uploads/2019/06/mikilife-300x225.png",
        value: "b"
      }
    ],
    val: null
  }),
  methods: {
    asyncData: async function({ app }) {
      const response = await app.$axios.$get('https://script.google.com/macros/s/AKfycbwBX-3XgnXvKuzhr_DTqMgOwHmrQPuTaa4J22iZN1k2fi1WkHY/exec');
      return { host: response};
    }
  },
  async mounted(){

    const response = await this.$axios.$get('https://script.google.com/macros/s/AKfycbwBX-3XgnXvKuzhr_DTqMgOwHmrQPuTaa4J22iZN1k2fi1WkHY/exec');
    this.val = response.val
    console.log(response)
  }
}
</script>
