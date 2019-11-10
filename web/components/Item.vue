<template>
  <div>
    <v-row>
    <v-col>
    <v-card

      dark
    >
      <div class="d-flex flex-no-wrap justify-start">
      <v-avatar
          class="ma-3"
          size="125"
          tile
        >
          <v-img :src='image_url'></v-img>
        </v-avatar>
        <div>
          <v-card-title
            class="headline"
            v-text="name"
          ></v-card-title>

          <v-card-subtitle v-text="description"></v-card-subtitle>
        </div>


      </div>
       <v-card-actions>


      <v-spacer></v-spacer>
      <v-btn
      v-show="currentFlag"
        color="red"
        text
      >
        現在選択中
      </v-btn>
      <v-btn
        color="orange"
        text
        @click="asyncData(value)"
      >
        これを与える
      </v-btn>

    </v-card-actions>

    </v-card>
    </v-col>
    </v-row>
 </div>
</template>

<style>

</style>
<script>
  export default {
    data: () => ({
    }),
    props: {
      name: String,
      description: String,
      image_url: String,
      value: String,
      currentFlag: Boolean
    },
  methods: {
    asyncData: async function(val) {
      const response = await this.$axios.$get(`https://script.google.com/macros/s/AKfycbwBX-3XgnXvKuzhr_DTqMgOwHmrQPuTaa4J22iZN1k2fi1WkHY/exec?text=${val}`);
      return { host: response};
    }
  },
  }
</script>
